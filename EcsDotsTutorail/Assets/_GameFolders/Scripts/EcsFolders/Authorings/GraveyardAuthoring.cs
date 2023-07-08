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
            AddComponent(new GraveyardDataComponent()
            {
                FieldDimension = authoring.FieldDimension,
                NumberTombstoneToSpawn = authoring.NumberTombstoneToSpawn,
                TombStonePrefab = GetEntity(authoring.Prefab, TransformUsageFlags.None)
            });
            
            AddComponent(new GraveyardRandomDataComponent()
            {
                Value = Random.CreateFromIndex(authoring.RandomSeed)
            });
        }
    }
}