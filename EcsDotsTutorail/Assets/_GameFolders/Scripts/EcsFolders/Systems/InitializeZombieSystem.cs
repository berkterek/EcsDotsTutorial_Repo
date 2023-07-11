using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var zombieWalkAspect in SystemAPI.Query<ZombieWalkAspect>().WithAll<ZombieTagData>())
            {
                entityCommandBuffer.RemoveComponent<ZombieTagData>(zombieWalkAspect.Entity);
                entityCommandBuffer.SetComponentEnabled<ZombieWalkData>(zombieWalkAspect.Entity, false);
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}