using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

public class FireSystem : SystemBase
{
	private EndSimulationEntityCommandBufferSystem EndSimECBSystem;

	protected override void OnCreate()
	{
		EndSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		EntityCommandBuffer.Concurrent ECB = EndSimECBSystem.CreateCommandBuffer().ToConcurrent();
		float deltaTime = Time.DeltaTime;

		Entities
			.ForEach((int entityInQueryIndex, ref FireCooldown fireCooldown, in TurretInput input, in FireInterval fireInterval, in FireSpeed fireSpeed, in LocalToWorld localToWorld, in ProjectilePrefab projectilePrefab, in ProjectileSpawnPoint spawnPointData) =>
			{
				//decrease the cooldown
				if(fireCooldown.Value > 0f)
				{
					fireCooldown.Value -= deltaTime;
				}

				if(input.Fire
					&& fireCooldown.Value <= 0f)
				{
					//fire!
					Entity newProjectile = ECB.Instantiate(entityInQueryIndex, projectilePrefab.Reference);

					//override a few components to position, rotate and push the newly created bullet
					ECB.SetComponent<Translation>(entityInQueryIndex, newProjectile, new Translation{Value = math.transform(localToWorld.Value, spawnPointData.LocalTranslation)});
					quaternion worldRotation = math.mul(localToWorld.Rotation, spawnPointData.LocalRotation);
					ECB.SetComponent<Rotation>(entityInQueryIndex, newProjectile, new Rotation{Value = worldRotation});
					ECB.SetComponent<PhysicsVelocity>(entityInQueryIndex, newProjectile, new PhysicsVelocity{Linear = fireSpeed.Value * math.forward(worldRotation)});

					fireCooldown.Value = fireInterval.Value;
				}
			}).ScheduleParallel();

		EndSimECBSystem.AddJobHandleForProducer(this.Dependency);
	}
}