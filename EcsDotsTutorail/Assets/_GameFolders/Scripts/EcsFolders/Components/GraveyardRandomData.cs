using Unity.Entities;
using Unity.Mathematics;

namespace EcsDotsTutorial.Components
{
    public struct GraveyardRandomData : IComponentData
    {
        public Random Value;
    }
}