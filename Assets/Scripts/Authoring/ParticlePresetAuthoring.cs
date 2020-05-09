using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;
using GD.MinMaxSlider;

public class ParticlePresetAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
	public GameObject particlePrefab;
	public int numberOfParticles = 10;
	[MinMaxSlider(0f, 10f)]public Vector2 initialSpeedRange = new Vector2(3f, 8f);
	[MinMaxSlider(.1f, 5f)]public Vector2 lifetimeRange = new Vector2(1f, 3f);

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData<ParticleEffectPreset>(entity, new ParticleEffectPreset{ParticlePrefab = conversionSystem.GetPrimaryEntity(particlePrefab),
																							NumberOfParticles = numberOfParticles,
																							InitialSpeedRange = initialSpeedRange,
																							LifetimeRange = lifetimeRange});
	}

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		if(particlePrefab != null)
		{
			referencedPrefabs.Add(particlePrefab);
		}
		else
		{
			Debug.LogError("No particle Prefab was selected for this ParticlePreset");
		}
	}
}