using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class TurretAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	public GameObject projectilePrefab;
	public Transform projectileSpawnPoint;
	[Range(.2f, 10f)] public float fireInterval = 1f;
	[Range(10f, 30f)] public float fireSpeed = 20f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
		dstManager.AddComponent<TurretInput>(entity);
		dstManager.AddComponentData(entity, new FireInterval{Value = fireInterval});
		dstManager.AddComponentData<FireCooldown>(entity, new FireCooldown{Value = fireInterval});
		dstManager.AddComponentData(entity, new FireSpeed{Value = fireSpeed});

		//store a reference to the entity which represents the projectile Prefab
		dstManager.AddComponentData(entity, new ProjectilePrefab{Reference = conversionSystem.GetPrimaryEntity(projectilePrefab)});

		//store the coordinates of the spawn point, so projectiles can be placed and rotated correctly when firing
		dstManager.AddComponentData(entity, new ProjectileSpawnPoint{LocalTranslation = projectileSpawnPoint.localPosition, LocalRotation = projectileSpawnPoint.localRotation});
    }

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		//declare that the projectile Prefab needs to be converted to an entity too
		referencedPrefabs.Add(projectilePrefab);
	}
}
