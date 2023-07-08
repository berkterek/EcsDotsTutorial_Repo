using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct ZombieSpawnPointsConfig : IComponentData
    {
        public NativeArray<float3> Values;
    }

    public struct ZombieSpawnConfigAsset : IComponentData
    {
        public BlobAssetReference<ZombieSpawnPointsConfig> Config;
    }
}