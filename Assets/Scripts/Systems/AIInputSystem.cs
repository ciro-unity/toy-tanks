using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class AIInputSystem : SystemBase
{
	protected override void OnUpdate()
	{
		float2 desiredTarget = float2.zero;
		float dt = Time.DeltaTime;

		//find the player entity and compute its intended position for this frame
		Entities.WithAll<PlayerTag, BodyInput>()
		.ForEach((in Translation translation, in Velocity velocity) =>
		{
			float3 worldSpacePoint = translation.Value + velocity.Value * dt;
			desiredTarget = new float2(worldSpacePoint.x, worldSpacePoint.z);
		}).Run();

		//let all AI tanks use the position as their target
		Entities
		.WithAll<EnemyTag>()
		.ForEach((ref TurretInput input) =>
		{
			input.Target = desiredTarget;
		}).Run();
	}
}