using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        const float SAFETY_RADIUS_SQ = 100f;
        
        public readonly Entity Entity;
        readonly RefRO<LocalTransform> _localTransformRO;
        readonly RefRO<GraveyardDataComponent> _graveyardRO;
        readonly RefRW<GraveyardRandomDataComponent> _graveyardRandomRW;

        public int NumberTombstonesToSpawn => _graveyardRO.ValueRO.NumberTombstoneToSpawn;
        public Entity TombStonePrefab => _graveyardRO.ValueRO.TombStonePrefab;

        float3 MinCorner => _localTransformRO.ValueRO.Position - HalfDimension;
        float3 MaxCorner => _localTransformRO.ValueRO.Position + HalfDimension;

        float3 HalfDimension => new float3
        (
            _graveyardRO.ValueRO.FieldDimension.x * 0.5f,
            0f,
            _graveyardRO.ValueRO.FieldDimension.y * 0.5f
        );

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