using Unity.Entities;

namespace EcsDotsTutorial.Components
{
    public struct ZombieWalkData : IComponentData
    {
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
    }

    public struct ZombieWalkTimerData : IComponentData
    {
        public float WalkTimerValue;
    }

    public struct ZombieHeadingData : IComponentData
    {
        public float HeadingValue;
    }
}