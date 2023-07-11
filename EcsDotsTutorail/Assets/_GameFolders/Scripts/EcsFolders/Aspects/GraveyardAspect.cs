using EcsDotsTutorial.Components;
using EcsDotsTutorial.Helpers;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        const float SAFETY_RADIUS_SQ = 100f;

        readonly RefRO<LocalTransform> _localTransformRO;
        readonly RefRO<GraveyardData> _graveyardRO;
        readonly RefRW<GraveyardRandomData> _graveyardRandomRW;
        readonly RefRW<ZombieSpawnPointsData> _zombieSpawnPointsRW;
        readonly RefRW<ZombieSpawnTimerData> _zombieSpawnTimerRW;

        public BlobArray<float3> ZombieSpawnPoints => _zombieSpawnPointsRW.ValueRO.Config.Value.Values;
        public int NumberTombstonesToSpawn => _graveyardRO.ValueRO.NumberTombstoneToSpawn;
        public Entity TombStonePrefab => _graveyardRO.ValueRO.TombStonePrefab;
        public Entity ZombiePrefab => _graveyardRO.ValueRO.ZombiePrefab;
        public bool TimeToSpawnZombie => ZombieSpawnCurrentRate <= 0f;
        public float ZombieSpawnRate => _graveyardRO.ValueRO.ZombieSpawnRate;
        public float ZombieSpawnCurrentRate
        {
            get => _zombieSpawnTimerRW.ValueRO.CurrentSpawnTime;
            set => _zombieSpawnTimerRW.ValueRW.CurrentSpawnTime = value;
        }

        int ZombieSpawnPointCount => _zombieSpawnPointsRW.ValueRO.Config.Value.Values.Length;
        float3 MinCorner => _localTransformRO.ValueRO.Position - HalfDimension;
        float3 MaxCorner => _localTransformRO.ValueRO.Position + HalfDimension;
        float3 HalfDimension => new float3
        (
            _graveyardRO.ValueRO.FieldDimension.x * 0.5f,
            0f,
            _graveyardRO.ValueRO.FieldDimension.y * 0.5f
        );
        
         public bool ZombieSpawnPointInitialized()
         {
             return _zombieSpawnPointsRW.ValueRO.Config.IsCreated && ZombieSpawnPointCount > 0;
         }

         public LocalTransform GetZombieRandomSpawnPoint()
         {
             var randomPosition = GetRandomZombieSpawnPoint();
             return new LocalTransform()
             {
                 Position = randomPosition,
                 Rotation = quaternion.RotateY(MathHelper.GetHeading(randomPosition, float3.zero)),
                 Scale = 1f
             };
         }

         private float3 GetRandomZombieSpawnPoint()
         {
             return GetZombieSpawnPoint(_graveyardRandomRW.ValueRW.Value.NextInt(ZombieSpawnPointCount));
         }

        private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPointsRW.ValueRO.Config.Value.Values[i];

        public LocalTransform GetRandomLocalTransform()
        {
            return new LocalTransform()
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale()
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = _graveyardRandomRW.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(randomPosition, _localTransformRO.ValueRO.Position) <= SAFETY_RADIUS_SQ);

            return randomPosition;
        }

        private quaternion GetRandomRotation()
        {
            return quaternion.RotateY(_graveyardRandomRW.ValueRW.Value.NextFloat(-0.35f, 0.35f));
        }

        private float GetRandomScale()
        {
            return _graveyardRandomRW.ValueRW.Value.NextFloat(0.5f, 1f);
        }
    }
}