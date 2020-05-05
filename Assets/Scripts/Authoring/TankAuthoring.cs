using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class TankAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [Range(1f, 4f)] public float movementSpeed = 3f;
	[Range(1f, 3f)] public float rotationSpeed = 2f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponentData(entity, new BodyInput());
		dstManager.AddComponentData(entity, new MovementSpeed {Value = movementSpeed});
		dstManager.AddComponentData(entity, new RotationSpeed {Value = rotationSpeed});
        dstManager.AddComponentData(entity, new Velocity());
    }
}
