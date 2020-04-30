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
		.ForEach((ref Rotation rotation, ref Translation translation, in LocalToWorld localToWorld, in TurretInput input) =>
		{
			float3 worldTarget = new float3(input.Target.x, 0f, input.Target.y);
			float3 worldPos = new float3(localToWorld.Position.x, 0f, localToWorld.Position.z);

			//calculate world direction
			float3 dir = math.normalize(worldTarget - worldPos);

			//TODO: convert it to local!

			rotation.Value = quaternion.LookRotation(dir, math.up());
		}).Schedule();
	}
}