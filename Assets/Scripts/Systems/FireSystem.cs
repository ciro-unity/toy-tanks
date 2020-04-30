using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

public class FireSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
		.WithStructuralChanges()
		.ForEach((ref TurretInput input, ref FireCooldown fireCooldown, in FireInterval fireInterval, in LocalToWorld localToWorld, in ProjectilePrefab projectilePrefab) =>
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
				EntityManager.SetComponentData(newProjectile, new Translation{Value = localToWorld.Position}); //TODO fix position
			}
		}).Run();
	}
}