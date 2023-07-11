using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace EcsDotsTutorial.Systems
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(ZombieSpawnSystem))]
    [BurstCompile]
    public partial struct ZombieRiseSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ZombieRiseTimerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferSingleton =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ZombieRiseJob()
            {
                DeltaTime = deltaTime,
                EntityCommandBufferSingleton= entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferSingleton;
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ZombieRiseAspect zombieRiseAspect, [ChunkIndexInQuery]int sortKey)
        {
            zombieRiseAspect.RiseProcess(DeltaTime);

            if (zombieRiseAspect.IsAboveGround)
            {
                zombieRiseAspect.SetOnGround();
                EntityCommandBufferSingleton.RemoveComponent<ZombieSpawnTimerData>(sortKey,zombieRiseAspect.Entity);
            }
        }
    }
}