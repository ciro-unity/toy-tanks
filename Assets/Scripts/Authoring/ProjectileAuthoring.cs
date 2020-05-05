using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ProjectileAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [Range(6f, 10f)] public float movementSpeed = 10f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponent<ProjectileTag>(entity);
		dstManager.AddComponentData(entity, new MovementSpeed {Value = movementSpeed});
    }
}
