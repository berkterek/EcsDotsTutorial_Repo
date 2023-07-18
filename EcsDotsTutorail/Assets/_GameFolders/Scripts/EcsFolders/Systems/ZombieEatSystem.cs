using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(ZombieWalkSystem))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct ZombieEatSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BrainTag>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ZombieEatData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainSq = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale * 5f + 1f;

            new ZombieEatJob()
            {
                DeltaTime = deltaTime,
                BrainEntity = brainEntity,
                BrainRadiusSq = brainSq,
                EntityCommandBufferSystem = entityCommandBuffer.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieEatJob : IJobEntity
    {
        public float DeltaTime;
        public Entity BrainEntity;
        public float BrainRadiusSq;
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferSystem;

        [BurstCompile]
        private void Execute(ZombieEatAspect zombieEatAspect, [ChunkIndexInQuery] int sortKey)
        {
            if (zombieEatAspect.IsEatingRange(float3.zero, BrainRadiusSq))
            {
                zombieEatAspect.Eat(DeltaTime, EntityCommandBufferSystem, sortKey, BrainEntity);    
            }
            else
            {
                EntityCommandBufferSystem.SetComponentEnabled<ZombieEatData>(sortKey,zombieEatAspect.Entity, false);
                EntityCommandBufferSystem.SetComponentEnabled<ZombieWalkData>(sortKey,zombieEatAspect.Entity, true);
            }
        }
    }
}