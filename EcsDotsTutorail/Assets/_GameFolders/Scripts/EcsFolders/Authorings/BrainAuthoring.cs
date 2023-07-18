using EcsDotsTutorial.Components;
using Unity.Entities;
using UnityEngine;

namespace EcsDotsTutorial.Authorings
{
    public class BrainAuthoring : MonoBehaviour
    {
        
    }

    public class BrainBaker : Baker<BrainAuthoring>
    {
        public override void Bake(BrainAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<BrainTag>(entity);
        }
    }
}