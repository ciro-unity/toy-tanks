using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(CollisionSystem))]
[UpdateAfter(typeof(ParticleSystem))]
public class CleanupSystem : SystemBase
{
	private EntityCommandBufferSystem ECBSystem;

	protected override void OnCreate()
	{
		ECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		EntityCommandBuffer.Concurrent ECB = ECBSystem.CreateCommandBuffer().ToConcurrent();

		Entities
			.WithAll<DestroyTag>()
			.ForEach((int entityInQueryIndex, Entity entity) =>
			{
				ECB.DestroyEntity(entityInQueryIndex, entity);
			}).ScheduleParallel();

		ECBSystem.AddJobHandleForProducer(this.Dependency);
	}
}