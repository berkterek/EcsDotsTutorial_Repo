using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct ZombieSpawnPointsBlobData
    {
        public BlobArray<float3> Values;
    }

    public struct ZombieSpawnPointsReference : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsBlobData> Config;
    }
}