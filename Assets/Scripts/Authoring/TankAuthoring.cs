using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class TankAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float speed = 10f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponentData(entity, new BodyInput());
		dstManager.AddComponentData(entity, new Speed {Value = speed});
        dstManager.AddComponentData(entity, new Velocity());
    }
}
