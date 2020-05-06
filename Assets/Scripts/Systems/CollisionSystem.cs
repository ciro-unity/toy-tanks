using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;

public class CollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
	private EndSimulationEntityCommandBufferSystem EndSimECBSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
		EndSimECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
		EntityCommandBuffer EndSimECB = EndSimECBSystem.CreateCommandBuffer();

        var triggerJob = new TriggerJob
        {
            enemiesGroup = GetComponentDataFromEntity<EnemyTag>(),
			playersGroup = GetComponentDataFromEntity<PlayerTag>(),
            projectilesGroup = GetComponentDataFromEntity<ProjectileTag>(),
			ECB = EndSimECB,
        };
        var jobHandle = triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();

        return jobHandle;
    }

    private struct TriggerJob : ITriggerEventsJob
    {
        //Query for the components we care about (as in ECS, we do stuff based on the components)
        [ReadOnly]  public ComponentDataFromEntity<EnemyTag> enemiesGroup;
        [ReadOnly]  public ComponentDataFromEntity<ProjectileTag> projectilesGroup;
		[ReadOnly]  public ComponentDataFromEntity<PlayerTag> playersGroup;
		
		public EntityCommandBuffer ECB;

        //This function will be called every time there is a trigger collision in the game
        public void Execute(TriggerEvent triggerEvent)
        {
			//TODO: Allow the destruction of the player
			
			Entity entityA = triggerEvent.Entities.EntityA;
			Entity entityB = triggerEvent.Entities.EntityB;

			//check which is which
            if (enemiesGroup.HasComponent(entityA))
            {
                if (projectilesGroup.HasComponent(entityB))
                {
					 ECB.DestroyEntity(entityA);
					 ECB.DestroyEntity(entityB);
                }
            }
			else if (enemiesGroup.HasComponent(triggerEvent.Entities.EntityB))
            {
                if (projectilesGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
					 ECB.DestroyEntity(entityA);
					 ECB.DestroyEntity(entityB);
                }
            }
        }
    }


}