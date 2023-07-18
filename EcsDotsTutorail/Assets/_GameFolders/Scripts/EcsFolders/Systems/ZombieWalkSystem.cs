using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferSingleton =
                SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            new ZombieWalkJob()
            {
                DeltaTime = deltaTime,
                BrainRadiusSq = 5.5f * 5.5f,
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
                Debug.Log("Zombie Walk Enable False " + sortKey);
                EntityCommandBufferParalleWriter.SetComponentEnabled<ZombieWalkData>(sortKey, zombieWalkAspect.Entity,
                    false);
            }
        }
    }
}