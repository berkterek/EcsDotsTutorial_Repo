using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
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
                Rotation = quaternion.identity,
                Scale = 1f
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition = _graveyardRandomRW.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);

            return randomPosition;
        }
    }
}