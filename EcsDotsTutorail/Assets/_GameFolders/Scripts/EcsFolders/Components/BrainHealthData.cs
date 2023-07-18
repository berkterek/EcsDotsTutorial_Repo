using Unity.Entities;

namespace EcsDotsTutorial.Components
{
    public struct BrainHealthData : IComponentData
    {
        public float Max;
        public float Current;
    }
}