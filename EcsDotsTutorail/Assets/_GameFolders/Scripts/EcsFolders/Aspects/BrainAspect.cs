using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct BrainAspect : IAspect
    {
        public readonly Entity Entity;
        readonly RefRW<LocalTransform> _localTransformRW;
        readonly RefRW<BrainHealthData> _brainHealthDataRW;
        readonly DynamicBuffer<BrainDamageBuffer> _brainDamageBuffer;

        public void DamageBrain()
        {
            foreach (var brainDamageBuffer in _brainDamageBuffer)
            {
                _brainHealthDataRW.ValueRW.Current -= brainDamageBuffer.Value;
            }
            
            _brainDamageBuffer.Clear();
        }
    }
}