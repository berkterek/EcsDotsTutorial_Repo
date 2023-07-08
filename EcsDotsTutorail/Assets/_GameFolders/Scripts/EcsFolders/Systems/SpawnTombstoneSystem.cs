using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem: ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardDataComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardDataComponent>();
            var graveyardAspect = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            int count = graveyardAspect.NumberTombstonesToSpawn;
            for (int i = 0; i < count; i++)
            {
                entityCommandBuffer.Instantiate(graveyardAspect.TombStonePrefab);
            }
            
            entityCommandBuffer.Playback(state.EntityManager);

            state.Enabled = false;
        }
    }
}