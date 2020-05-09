using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

public class ExplosionAuthoring : MonoBehaviour, IDeclareReferencedPrefabs
{
	public GameObject particle;

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		referencedPrefabs.Add(particle);
	}
}