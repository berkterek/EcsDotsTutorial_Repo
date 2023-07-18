using Unity.Entities;

namespace EcsDotsTutorial.Components
{
    public struct ZombieEatData : IComponentData, IEnableableComponent
    {
        public float EatSpeed;
        public float EatAmplitude;
        public float EatFrequency;
    }

    public struct ZombieEatTimerData : IComponentData
    {
        public float Value;
    }
}