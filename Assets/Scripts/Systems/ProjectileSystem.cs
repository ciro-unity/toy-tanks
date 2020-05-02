using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

public class ProjectileSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
		.WithAll<ProjectileTag>()
		.ForEach((ref Translation translation, in LocalToWorld localToWorld) =>
		{
			translation.Value += localToWorld.Forward * Time.DeltaTime;
		}).ScheduleParallel();
	}
}