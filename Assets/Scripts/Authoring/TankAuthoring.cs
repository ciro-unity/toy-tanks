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

	public PathAuthoring movementPath;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponentData(entity, new BodyInput());
		dstManager.AddComponentData(entity, new MovementSpeed {Value = movementSpeed});
		dstManager.AddComponentData(entity, new RotationSpeed {Value = rotationSpeed});
        dstManager.AddComponentData(entity, new Velocity());

		if(movementPath != null)
		{
			Entity pathEntity = conversionSystem.GetPrimaryEntity(movementPath);
			dstManager.AddComponentData(entity, new PathMovement{Path = pathEntity, CurrentTargetWaypoint = 0, IsLooping = true});
		}

		//we will use this when destroying the tank: upon destruction of the base Entity,
		//all of the Entities in the LinkedEntityGroup buffer will be destroyed too
		conversionSystem.DeclareLinkedEntityGroup(gameObject);
    }
}
