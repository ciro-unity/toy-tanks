using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

[DisableAutoCreation]
public class ProjectileSystem : SystemBase
{
	protected override void OnUpdate()
	{
		float deltaTime = Time.DeltaTime;

		Entities
		.WithAll<ProjectileTag>()
		.ForEach((ref Translation translation, ref Rotation rotation, ref PhysicsVelocity physicVelocity, in LocalToWorld localToWorld, in MovementSpeed speed) =>
		{
			physicVelocity.Linear = localToWorld.Forward * speed.Value * deltaTime * 40f;
			//rotation.Value = math.mul(rotation.Value, quaternion.RotateX(.003f));
		}).ScheduleParallel();
	}
}