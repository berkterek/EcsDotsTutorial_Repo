using EcsDotsTutorial.Aspects;
using Unity.Burst;
using Unity.Entities;
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
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            new ZombieWalkJob()
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ZombieWalkAspect zombieWalkAspect)
        {
            zombieWalkAspect.Walk(DeltaTime);
        }
    }
}