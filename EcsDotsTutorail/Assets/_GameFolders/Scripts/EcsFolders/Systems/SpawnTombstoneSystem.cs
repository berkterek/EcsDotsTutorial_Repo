using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

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
            var spawnPoints = new NativeList<float3>(Allocator.Temp);
            var tombstoneOffset = new float3(0f, -2f, 1f);
            int count = graveyardAspect.NumberTombstonesToSpawn;
            for (int i = 0; i < count; i++)
            {
                var spawnTombstoneEntity = entityCommandBuffer.Instantiate(graveyardAspect.TombStonePrefab);
                var tombstoneLocalTransform = graveyardAspect.GetRandomLocalTransform();
                entityCommandBuffer.SetComponent(spawnTombstoneEntity, tombstoneLocalTransform);
                var newZombieSpawnPoint = tombstoneLocalTransform.Position + tombstoneOffset;
                spawnPoints.Add(newZombieSpawnPoint);
            }
            
            graveyardAspect.SetSpawnPoints(spawnPoints.ToArray(Allocator.Persistent));
            entityCommandBuffer.Playback(state.EntityManager);

            state.Enabled = false;
        }
    }
}