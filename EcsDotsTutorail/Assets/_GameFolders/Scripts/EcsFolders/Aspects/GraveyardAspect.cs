using System.Collections;
using System.Collections.Generic;
using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace EcsDotsTutorial.Aspects
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;
        readonly RefRW<LocalTransform> _localTransformRW;
        readonly RefRO<GraveyardDataComponent> _graveyardRO;
        readonly RefRO<GraveyardRandomDataComponent> _graveyardRandomRO;
    }
}