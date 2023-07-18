using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity Entity;
        public readonly RefRW<LocalTransform> _localTransformRW;
        public readonly RefRO<ZombieWalkData> _zombieWalkDataRO;
        public readonly RefRW<ZombieWalkTimerData> _zombieWalkTimerDataRW;
        public readonly RefRO<ZombieHeadingData> _zombieHeadingDataRO;

        public float WalkSpeed => _zombieWalkDataRO.ValueRO.WalkSpeed;
        public float WalkAmplitude => _zombieWalkDataRO.ValueRO.WalkAmplitude;
        public float WalkFrequency => _zombieWalkDataRO.ValueRO.WalkFrequency;
        public float Heading => _zombieHeadingDataRO.ValueRO.HeadingValue;

        float WalkTimer
        {
            get => _zombieWalkTimerDataRW.ValueRO.WalkTimerValue;
            set => _zombieWalkTimerDataRW.ValueRW.WalkTimerValue = value;
        }

        public void Walk(float deltaTime)
        {
            WalkTimer += deltaTime;
            _localTransformRW.ValueRW.Position += deltaTime * _zombieWalkDataRO.ValueRO.WalkSpeed * _localTransformRW.ValueRW.Forward();

            var wayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
            _localTransformRW.ValueRW.Rotation = quaternion.Euler(0f, Heading, wayAngle);
        }

        public bool IsInStopRange(float3 brainPosition, float brainRadiusSq)
        {
            float distanceSqResult = math.distancesq(brainPosition, _localTransformRW.ValueRO.Position);
            return distanceSqResult <= brainRadiusSq;
        }
    }
}