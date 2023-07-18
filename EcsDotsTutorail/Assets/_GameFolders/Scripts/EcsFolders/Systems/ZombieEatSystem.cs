using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Entities;
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

            new ZombieEatJob()
            {
                DeltaTime = deltaTime,
                BrainEntity = brainEntity,
                EntityCommandBufferSystem = entityCommandBuffer.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
                
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieEatJob : IJobEntity
    {
        public float DeltaTime;
        public Entity BrainEntity;
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferSystem;

        [BurstCompile]
        private void Execute(ZombieEatAspect zombieEatAspect, [ChunkIndexInQuery] int sortKey)
        {
            zombieEatAspect.Eat(DeltaTime, EntityCommandBufferSystem, sortKey, BrainEntity);
        }
    }
}