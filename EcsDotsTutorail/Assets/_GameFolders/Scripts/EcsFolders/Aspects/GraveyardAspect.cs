using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;
        readonly RefRW<LocalTransform> _localTransformRW;
        readonly RefRO<GraveyardDataComponent> _graveyardRO;
        readonly RefRO<GraveyardRandomDataComponent> _graveyardRandomRO;

        public int NumberTombstonesToSpawn => _graveyardRO.ValueRO.NumberTombstoneToSpawn;
        public Entity TombStonePrefab => _graveyardRO.ValueRO.TombStonePrefab;
    }
}