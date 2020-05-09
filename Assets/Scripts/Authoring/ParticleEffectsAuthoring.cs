using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class ParticleEffectsAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
	public GameObject creationParticleEffect, destructionParticleEffect;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData<ParticleEffects>(entity, new ParticleEffects{CreationEffect = conversionSystem.GetPrimaryEntity(creationParticleEffect),
																				DestructionEffect = conversionSystem.GetPrimaryEntity(destructionParticleEffect)});
		
		if(creationParticleEffect != null) dstManager.AddComponent<EmitParticlesOnCreationTag>(entity);
		if(destructionParticleEffect != null) dstManager.AddComponent<EmitParticlesOnDestructionTag>(entity);
	}

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		if(creationParticleEffect != null)
			referencedPrefabs.Add(creationParticleEffect);

		if(destructionParticleEffect != null)
			referencedPrefabs.Add(destructionParticleEffect);
	}
}