using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class AIInputSystem : SystemBase
{
    private EntityQuery m_PlayerQuery;

    struct PosVel
    {
        public float2 pos;
        public float2 vel;
    }

    protected override void OnUpdate()
    {
        // aim 30 frames ahead, assume 60hz to avoid jitter
        float dt = .5f;

        var targetCount = m_PlayerQuery.CalculateEntityCount();

        if (targetCount > 0)
        {
            var targets = new NativeArray<PosVel>(targetCount, Allocator.TempJob); //created as a native array so it can be passed to the job below

            //find the player entity and compute its intended position for this frame
            Entities
                .WithAll<PlayerTag, BodyInput>()
                .WithStoreEntityQueryInField(ref m_PlayerQuery)
                .ForEach((int entityInQueryIndex, in Translation translation, in Rotation rotation, in Velocity velocity) =>
                {
                    targets[entityInQueryIndex] = new PosVel
                    {
                        pos = translation.Value.xz,
                        vel = math.forward(rotation.Value).xz * velocity.Value * 2f
                    };
                }).ScheduleParallel();

            //let all AI tanks use the position as their target
            Entities
                .WithAll<EnemyTag>()
                .WithDeallocateOnJobCompletion(targets)
                .ForEach((ref TurretInput input, in Translation translation) =>
                {
                    input.Target = targets[0].pos + targets[0].vel * dt;
                    input.Fire = true;
                }).ScheduleParallel();
        }
        else
        {
            Entities
                .WithAll<EnemyTag>()
                .ForEach((ref TurretInput input) =>
                {
                    input.Fire = false;
                }).ScheduleParallel();
        }
    }
}