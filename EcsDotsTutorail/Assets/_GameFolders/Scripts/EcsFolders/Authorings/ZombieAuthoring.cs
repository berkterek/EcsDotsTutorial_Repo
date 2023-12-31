﻿using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;

namespace EcsDotsTutorial.Authorings
{
    public class ZombieAuthoring : MonoBehaviour
    {
        public float ZombieRiseRate;
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequecy;
        
        public float EatSpeed;
        public float EatAmplitude;
        public float EatFrequecy;
    }
    
    public class ZombieBaker : Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            var zombieEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(zombieEntity, new ZombieRiseTimerData()
            {
                Value = authoring.ZombieRiseRate
            });
            
            AddComponent(zombieEntity, new ZombieWalkData()
            {
                WalkAmplitude = authoring.WalkAmplitude,
                WalkSpeed = authoring.WalkSpeed,
                WalkFrequency = authoring.WalkFrequecy
            });

            AddComponent(zombieEntity, new ZombieEatData()
            {
                EatAmplitude = authoring.EatAmplitude,
                EatSpeed = authoring.EatSpeed,
                EatFrequency = authoring.EatFrequecy
            });

            AddComponent<ZombieEatTimerData>(zombieEntity);
            AddComponent<ZombieWalkTimerData>(zombieEntity);
            AddComponent<ZombieHeadingData>(zombieEntity);
            AddComponent<ZombieTagData>(zombieEntity);
        }
    }
}