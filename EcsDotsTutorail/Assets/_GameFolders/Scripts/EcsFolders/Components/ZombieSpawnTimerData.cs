using Unity.Entities;

namespace EcsDotsTutorial.Components
{
    public struct ZombieSpawnTimerData : IComponentData
    {
        public float CurrentSpawnTime;
    }
}