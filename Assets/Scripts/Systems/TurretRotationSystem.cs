using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TurretRotationSystem : SystemBase
{
	protected override void OnUpdate()
	{

		Entities
		.ForEach((ref Rotation rotation, in Inputs inputs) =>
		{
			//TODO
		}).Schedule();
	}
}