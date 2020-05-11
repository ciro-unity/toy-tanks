using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

public class ParticleSystem : SystemBase
{
	private EntityCommandBufferSystem ECBSystem;

	protected override void OnCreate()
	{
		ECBSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		EntityCommandBuffer.Concurrent ECB = ECBSystem.CreateCommandBuffer().ToConcurrent();
		Random random = new Random(1);
		float deltaTime = Time.DeltaTime;

		//creation of new particle systems
		Entities
			.WithAny<EmitParticlesOnCreationTag, EmitParticlesOnDestructionTag>()
			.ForEach((int entityInQueryIndex, Entity entity, ref ParticleEffects effects, in Translation translation) =>
			{
				Entity newEmitter = Entity.Null;

				if(HasComponent<EmitParticlesOnCreationTag>(entity))
				{
					newEmitter = ECB.Instantiate(entityInQueryIndex, effects.CreationEffect);

					//remove the component or the particles will spawn every frame!
					ECB.RemoveComponent<EmitParticlesOnCreationTag>(entityInQueryIndex, entity);
				}

				if(HasComponent<EmitParticlesOnDestructionTag>(entity))
				{
					newEmitter = ECB.Instantiate(entityInQueryIndex, effects.DestructionEffect);

					//remove the System State Component, which will allow for the destruction of the entity
					ECB.RemoveComponent<EmitParticlesOnDestructionTag>(entityInQueryIndex, entity);
				}

				ECB.SetComponent<Translation>(entityInQueryIndex, newEmitter, translation); //same position as the body that emitted it

			}).ScheduleParallel();


		//if there's an emitter in the world, create all of its particles at once
		Entities
			.ForEach((int entityInQueryIndex, Entity entity, ref ParticleEffectPreset particleData, in Translation translation) =>
			{
				for(int i=0; i<particleData.NumberOfParticles; i++)
				{
					Entity newParticle = ECB.Instantiate(entityInQueryIndex, particleData.ParticlePrefab);
					ECB.AddComponent<Particle>(entityInQueryIndex, newParticle, new Particle{
														Lifetime = random.NextFloat(particleData.LifetimeRange.x,
																					particleData.LifetimeRange.y),
														ElapsedTime = 0f,
														MovementDirection = random.NextFloat3Direction(),
														Velocity = random.NextFloat(particleData.InitialSpeedRange.x,
																					particleData.InitialSpeedRange.y)});
					ECB.SetComponent<Translation>(entityInQueryIndex, newParticle, translation); //same position as the emitter
					ECB.AddComponent<Scale>(entityInQueryIndex, newParticle, new Scale{Value = 0f});
				}

				//remove the preset entity now that the particles have been spawned
				ECB.DestroyEntity(entityInQueryIndex, entity);

			}).ScheduleParallel();


		//move and scale all particles in the world
		Entities
			.ForEach((int entityInQueryIndex, Entity entity, ref Particle particle, ref Translation translation, ref Scale scale) =>
			{
				//add to the elapsed time, and destroy if past the particle's lifetime
				particle.ElapsedTime += deltaTime;
				if(particle.ElapsedTime >= particle.Lifetime)
				{
					ECB.DestroyEntity(entityInQueryIndex, entity);
				}
				else
				{
					//move and scale the particle
					translation.Value += particle.MovementDirection * particle.Velocity * deltaTime;
					scale.Value = math.sin((particle.ElapsedTime/particle.Lifetime) * math.PI);
				}
			}).ScheduleParallel();


		ECBSystem.AddJobHandleForProducer(this.Dependency);
	}
}