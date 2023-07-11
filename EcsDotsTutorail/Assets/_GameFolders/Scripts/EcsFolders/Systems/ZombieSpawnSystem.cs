using EcsDotsTutorial.Aspects;
using EcsDotsTutorial.Components;
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
            // state.RequireForUpdate<GraveyardData>();
            // state.RequireForUpdate<ZombieSpawnPointsBlobData>();
            // state.RequireForUpdate<ZombieSpawnTimerData>();
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
                EntityCommandBuffer = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }

    [BurstCompile]
    public partial struct ZombieSpawnJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        
        [BurstCompile]
        private void Execute(GraveyardAspect graveyardAspect)
        {
            graveyardAspect.ZombieSpawnCurrentRate -= DeltaTime;

            if (!graveyardAspect.TimeToSpawnZombie) return;
            if(!graveyardAspect.ZombieSpawnPointInitialized()) return;
            if(graveyardAspect.ZombieSpawnPoints.Length == 0) return;

            graveyardAspect.ZombieSpawnCurrentRate = graveyardAspect.ZombieSpawnRate;
            var zombieEntity = EntityCommandBuffer.Instantiate(graveyardAspect.ZombiePrefab);
        }
    }
}