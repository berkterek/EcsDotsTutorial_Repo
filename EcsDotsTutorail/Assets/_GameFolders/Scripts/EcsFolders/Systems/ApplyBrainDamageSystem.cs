using EcsDotsTutorial.Aspects;
using Unity.Burst;
using Unity.Entities;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ApplyBrainDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var brainAspect in SystemAPI.Query<BrainAspect>())
            {
                brainAspect.DamageBrain();
            }
        }
    }
}