using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class TurretAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	public GameObject projectilePrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponentData(entity, new TurretInput());

		//store a reference to the entity which represents the projectile Prefab
		dstManager.AddComponentData(entity, new ProjectilePrefab{Reference = conversionSystem.GetPrimaryEntity(projectilePrefab)});
    }

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		//declare that the projectile Prefab needs to be converted to an entity too
		referencedPrefabs.Add(projectilePrefab);
	}
}
