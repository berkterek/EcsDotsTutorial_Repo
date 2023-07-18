using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieRiseSystem))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct ZombieWalkSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BrainTag>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferSingleton =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainTag = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainScale = SystemAPI.GetComponent<LocalTransform>(brainTag).Scale;
            var brainRadius = brainScale * 5f + 0.5f;
            
            new ZombieWalkJob()
            {
                DeltaTime = deltaTime,
                BrainRadiusSq = brainRadius,
                EntityCommandBufferParalleWriter = entityCommandBufferSingleton
                    .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float DeltaTime;
        public float BrainRadiusSq;
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferParalleWriter;

        [BurstCompile]
        private void Execute(ZombieWalkAspect zombieWalkAspect, [ChunkIndexInQuery] int sortKey)
        {
            zombieWalkAspect.Walk(DeltaTime);

            if (zombieWalkAspect.IsInStopRange(float3.zero, BrainRadiusSq))
            {
                EntityCommandBufferParalleWriter.SetComponentEnabled<ZombieWalkData>(sortKey, zombieWalkAspect.Entity,
                    false);
                EntityCommandBufferParalleWriter.SetComponentEnabled<ZombieEatData>(sortKey, zombieWalkAspect.Entity, true);
            }
        }
    }
}