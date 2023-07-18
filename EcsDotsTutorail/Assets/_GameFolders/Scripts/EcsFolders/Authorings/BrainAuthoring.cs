using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;

namespace EcsDotsTutorial.Authorings
{
    public class BrainAuthoring : MonoBehaviour
    {
        public float MaxHealth;
    }

    public class BrainBaker : Baker<BrainAuthoring>
    {
        public override void Bake(BrainAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<BrainTag>(entity);
            AddBuffer<BrainDamageBuffer>(entity);
            AddComponent(entity, new BrainHealthData()
            {
                Max = authoring.MaxHealth,
                Current = authoring.MaxHealth
            });
        }
    }
}