using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class AIInputSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
		.WithAll<EnemyTag>()
		.ForEach((ref BodyInput input) =>
		{
			input.Movement = new float2(.5f, .1f); //mock data
		}).Run();

		Entities
		.WithAll<EnemyTag>()
		.ForEach((ref TurretInput input) =>
		{
			input.Target = new float2(0f, 0f); //mock data
			input.Fire = false;
		}).Run();
	}
}