using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct GraveyardDataComponent : IComponentData
    {
        public float2 FieldDimension;
        public int NumberTombstoneToSpawn;
        public Entity TombStonePrefab;
    }
}
