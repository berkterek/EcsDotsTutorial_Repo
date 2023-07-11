using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct ZombieSpawnPointsBlobData : IComponentData
    {
        public BlobArray<float3> Values;
    }

    public struct ZombieSpawnPointsData : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsBlobData> Config;
    }
}