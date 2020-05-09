using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

public class ParticleSystem : SystemBase
{
	private EndSimulationEntityCommandBufferSystem EndSimECBSystem;

	protected override void OnCreate()
	{
		EndSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		EntityCommandBuffer.Concurrent ECB = EndSimECBSystem.CreateCommandBuffer().ToConcurrent();
		Random random = new Random(0);

		Entities
			.WithAll<EmitParticlesTag>()
			.ForEach((int entityInQueryIndex, ref ParticleEffect particleData) =>
			{
				for(int i=0; i<particleData.NumberOfParticles; i++)
				{
					Entity newParticle = ECB.Instantiate(entityInQueryIndex, particleData.ParticlePrefab);
					ECB.SetComponent<Particle>(entityInQueryIndex, newParticle, new Particle{
														Lifetime = random.NextFloat(particleData.LifetimeRange.x,
																					particleData.LifetimeRange.y),
														ElapsedTime = 0f,
														MovementDirection = random.NextFloat3Direction(),
														Velocity = random.NextFloat(particleData.InitialSpeedRange.x,
																					particleData.InitialSpeedRange.y)});
				}
			}).ScheduleParallel();

		EndSimECBSystem.AddJobHandleForProducer(this.Dependency);
	}
}