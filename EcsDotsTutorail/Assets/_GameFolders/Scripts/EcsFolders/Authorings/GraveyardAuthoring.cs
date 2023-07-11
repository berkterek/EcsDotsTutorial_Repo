using System;
using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EcsDotsTutorial.Authorings
{
    public class GraveyardAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public int NumberTombstoneToSpawn;
        public Vector2 FieldDimension;
        public uint RandomSeed;
    }
    
    public class GraveyardBaker : Baker<GraveyardAuthoring>
    {
        public override void Bake(GraveyardAuthoring authoring)
        {
            var graveyardEntity = GetEntity(TransformUsageFlags.Dynamic); 
            AddComponent(graveyardEntity, new GraveyardDataComponent()
            {
                FieldDimension = authoring.FieldDimension,
                NumberTombstoneToSpawn = authoring.NumberTombstoneToSpawn,
                TombStonePrefab = GetEntity(authoring.Prefab, TransformUsageFlags.None)
            });
            
            AddComponent(graveyardEntity, new GraveyardRandomDataComponent()
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed)
            });
            
            AddComponent<ZombieSpawnPointsReference>(graveyardEntity);
        }
    }
}