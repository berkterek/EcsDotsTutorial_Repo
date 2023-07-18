using EcsDotsTutorial.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace EcsDotsTutorial.Systems
{
    public partial class CameraControllerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainScale = SystemAPI.GetComponent<LocalTransform>(brainEntity).Scale;

            var cameraSingleton = CameraController.Instance;

            if (cameraSingleton == null) return;

            var positionFactor = (float)SystemAPI.Time.ElapsedTime * cameraSingleton.Speed;
            
            var radius =cameraSingleton.RadiusAtScale(brainScale);
            var height = cameraSingleton.HeightAtScale(brainScale);

            cameraSingleton.transform.position = new Vector3()
            {
                x = Mathf.Cos(positionFactor) * radius,
                y = height,
                z = Mathf.Sin(positionFactor) * radius
            };
            
            cameraSingleton.transform.LookAt(Vector3.zero);
        }
    }
}