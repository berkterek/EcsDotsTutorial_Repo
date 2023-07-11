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
    public partial struct SpawnTombstoneSystem : ISystem
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
            var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints = ref builder.ConstructRoot<ZombieSpawnPointsBlobData>();
            int count = graveyardAspect.NumberTombstonesToSpawn;
            var arrayBuilder = builder.Allocate(ref spawnPoints.Values, count);

            var tombstoneOffset = new float3(0f, -2f, 1f);
            for (int i = 0; i < count; i++)
            {
                var spawnTombstoneEntity = entityCommandBuffer.Instantiate(graveyardAspect.TombStonePrefab);
                var tombstoneLocalTransform = graveyardAspect.GetRandomLocalTransform();
                entityCommandBuffer.SetComponent(spawnTombstoneEntity, tombstoneLocalTransform);
                var newZombieSpawnPoint = tombstoneLocalTransform.Position + tombstoneOffset;
                arrayBuilder[i] = newZombieSpawnPoint;
            }

            var blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointsBlobData>(Allocator.Persistent);
            entityCommandBuffer.SetComponent(graveyardEntity, new ZombieSpawnPointsReference()
            {
                Config = blobAsset
            });
            entityCommandBuffer.Playback(state.EntityManager);

            builder.Dispose();
            state.Enabled = false;
        }
    }
}