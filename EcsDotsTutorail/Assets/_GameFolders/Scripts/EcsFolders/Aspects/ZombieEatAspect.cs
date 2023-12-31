﻿using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct ZombieEatAspect : IAspect
    {
        public readonly Entity Entity;

        readonly RefRW<LocalTransform> _localTransformRW;
        readonly RefRW<ZombieEatTimerData> _zombieTimerDataRW;
        readonly RefRO<ZombieEatData> _zombieEatDataRO;
        readonly RefRO<ZombieHeadingData> _zombieHeadingRO;

        public float EatDamagePerSecond => _zombieEatDataRO.ValueRO.EatSpeed;
        public float EatAmplitude => _zombieEatDataRO.ValueRO.EatAmplitude;
        public float EatFrequency => _zombieEatDataRO.ValueRO.EatFrequency;
        public float Heading => _zombieHeadingRO.ValueRO.HeadingValue;

        public float ZombieTimer
        {
            get => _zombieTimerDataRW.ValueRO.Value;
            set => _zombieTimerDataRW.ValueRW.Value = value;
        }

        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter entityCommandBuffer, int sortKey, Entity entityBrain)
        {
            ZombieTimer += deltaTime;
            var eatAngle = EatAmplitude * math.sin(EatFrequency * ZombieTimer);
            _localTransformRW.ValueRW.Rotation = quaternion.Euler(eatAngle, Heading, 0f);

            var eatDamage = EatDamagePerSecond * deltaTime;
            var currentBrainDamage = new BrainDamageBuffer()
            {
                Value = eatDamage
            };
            
            entityCommandBuffer.AppendToBuffer(sortKey, entityBrain, currentBrainDamage);
        }

        public bool IsEatingRange(float3 brainPosition, float brainRadiusSq)
        {
            var distanceSq = math.distancesq(brainPosition, _localTransformRW.ValueRO.Position);
            return distanceSq <= brainRadiusSq - 1f;
        }
    }
}