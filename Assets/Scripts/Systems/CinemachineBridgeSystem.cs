using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class CinemachineBridgeSystem : SystemBase
{
	Transform cmReferenceTarget;

	protected override void OnCreate()
	{
		cmReferenceTarget = GameObject.FindObjectOfType<CinemachineBridge>().transform;
	}

	protected override void OnUpdate()
	{
		NativeArray<float3> entityPosition = new NativeArray<float3>(1, Allocator.Temp);
		
		Entities
			.WithAll<PlayerTag, BodyInput>()
			.ForEach((in Translation translation) =>
			{
				entityPosition[0] = translation.Value;
			}).Run();

		cmReferenceTarget.transform.position = entityPosition[0];

		entityPosition.Dispose();
	}
}