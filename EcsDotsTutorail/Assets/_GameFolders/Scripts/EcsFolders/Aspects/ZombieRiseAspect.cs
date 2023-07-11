using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct ZombieRiseAspect : IAspect
    {
        public readonly RefRW<LocalTransform> LocalTransformRW;
        public readonly RefRO<ZombieRiseTimerData> ZombieRiseTimerDataRO;

        public void RiseProcess(float deltaTime)
        {
            LocalTransformRW.ValueRW.Position +=
                deltaTime * ZombieRiseTimerDataRO.ValueRO.Value * math.up();
        }
    }
}