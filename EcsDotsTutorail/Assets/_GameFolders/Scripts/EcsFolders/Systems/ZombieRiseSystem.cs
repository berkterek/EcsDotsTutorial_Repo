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
            state.RequireForUpdate<ZombieRiseTimerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            new ZombieRiseJob()
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ZombieRiseAspect zombieRiseAspect)
        {
            zombieRiseAspect.RiseProcess(DeltaTime);
        }
    }
}