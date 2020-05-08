using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

public class FireSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
			.WithStructuralChanges()
			.ForEach((ref FireCooldown fireCooldown, in TurretInput input, in Rotation rotation, in FireInterval fireInterval, in FireSpeed fireSpeed, in LocalToWorld localToWorld, in ProjectilePrefab projectilePrefab, in ProjectileSpawnPoint spawnPointData) =>
			{
				//decrease the cooldown
				if(fireCooldown.Value > 0f)
				{
					fireCooldown.Value -= Time.DeltaTime;
				}

				if(input.Fire
					&& fireCooldown.Value <= 0f)
				{
					//fire!
					Entity newProjectile = EntityManager.Instantiate(projectilePrefab.Reference);
					EntityManager.SetComponentData(newProjectile, new Translation{Value = math.transform(localToWorld.Value, spawnPointData.LocalTranslation)});
					quaternion worldRotation = math.mul(localToWorld.Rotation, spawnPointData.LocalRotation);
					EntityManager.SetComponentData(newProjectile, new Rotation{Value = worldRotation});
					EntityManager.SetComponentData(newProjectile, new PhysicsVelocity{Linear = fireSpeed.Value * math.forward(worldRotation)});

					fireCooldown.Value = fireInterval.Value;
				}
			}).Run();
	}
}