using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct GraveyardRandomDataComponent : IComponentData
    {
        public Random Value;
    }
}