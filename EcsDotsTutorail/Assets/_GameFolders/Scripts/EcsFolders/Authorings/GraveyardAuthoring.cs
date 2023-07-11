using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EcsDotsTutorial.Authorings
{
    public class GraveyardAuthoring : MonoBehaviour
    {
        public GameObject TombstonePrefab;
        public int NumberTombstoneToSpawn;
        public Vector2 FieldDimension;
        public uint RandomSeed;
        public GameObject ZombiePrefab;
        public float ZombieSpawnRate;
    }
    
    public class GraveyardBaker : Baker<GraveyardAuthoring>
    {
        public override void Bake(GraveyardAuthoring authoring)
        {
            var graveyardEntity = GetEntity(TransformUsageFlags.Dynamic); 
            AddComponent(graveyardEntity, new GraveyardData()
            {
                FieldDimension = authoring.FieldDimension,
                NumberTombstoneToSpawn = authoring.NumberTombstoneToSpawn,
                TombStonePrefab = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Renderable),
                ZombiePrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic),
                ZombieSpawnRate = authoring.ZombieSpawnRate
            });
            
            AddComponent(graveyardEntity, new GraveyardRandomData()
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed)
            });
            
            AddComponent<ZombieSpawnPointsData>(graveyardEntity);
            AddComponent<ZombieSpawnTimerData>(graveyardEntity);
        }
    }
}