using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
public class AIInputMockSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
			.WithAll<EnemyTag>()
			.ForEach((ref BodyInput input) =>
			{
				input.Movement = new float2(.5f, .1f); //mock data
			}).ScheduleParallel();

		Entities
			.WithAll<EnemyTag>()
			.ForEach((ref TurretInput input) =>
			{
				input.Target = new float2(0f, 0f); //mock data
				input.Fire = false;
			}).ScheduleParallel();
	}
}