﻿using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
using EcsDotsTutorial.Helpers;
using Unity.Burst;
using Unity.Entities;

namespace EcsDotsTutorial.Systems
{
    [BurstCompile]
    public partial struct ZombieSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferSingleton =
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new ZombieSpawnJob()
            {
                DeltaTime = deltaTime,
                EntityCommandBufferSingleton = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }

    [BurstCompile]
    public partial struct ZombieSpawnJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBufferSingleton;
        
        [BurstCompile]
        private void Execute(GraveyardAspect graveyardAspect)
        {
            graveyardAspect.ZombieSpawnCurrentRate -= DeltaTime;

            if (!graveyardAspect.TimeToSpawnZombie) return;
            if(!graveyardAspect.ZombieSpawnPointInitialized()) return;
            if(graveyardAspect.ZombieSpawnPoints.Length == 0) return;

            graveyardAspect.ZombieSpawnCurrentRate = graveyardAspect.ZombieSpawnRate;
            
            var zombieEntity = EntityCommandBufferSingleton.Instantiate(graveyardAspect.ZombiePrefab);
            var newZombieRandomPosition = graveyardAspect.GetZombieRandomSpawnPoint();
            EntityCommandBufferSingleton.SetComponent(zombieEntity, newZombieRandomPosition);

            var zombieHeading = MathHelper.GetHeading(newZombieRandomPosition.Position, graveyardAspect.Position);
            EntityCommandBufferSingleton.SetComponent(zombieEntity, new ZombieHeadingData()
            {
                HeadingValue = zombieHeading
            });
        }
    }
}