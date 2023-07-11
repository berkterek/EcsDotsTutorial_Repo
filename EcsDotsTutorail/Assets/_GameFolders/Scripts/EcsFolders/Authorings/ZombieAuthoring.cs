using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;

namespace EcsDotsTutorial.Authorings
{
    public class ZombieAuthoring : MonoBehaviour
    {
        public float ZombieRiseRate;
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
        }
    }
}