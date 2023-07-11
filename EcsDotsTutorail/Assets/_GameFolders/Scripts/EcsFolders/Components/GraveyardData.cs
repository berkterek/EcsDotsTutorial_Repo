using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct GraveyardData : IComponentData
    {
        public float2 FieldDimension;
        public int NumberTombstoneToSpawn;
        public Entity TombStonePrefab;
        public Entity ZombiePrefab;
        public float ZombieSpawnRate;
    }
}
