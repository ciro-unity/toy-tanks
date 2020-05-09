using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//Writes the BodyInput for enemy tanks that have a path to walk on

public class AIBodyInputPathSystem : SystemBase
{
    protected override void OnUpdate()
    {
		BufferFromEntity<Waypoint> allWaypointBuffers = GetBufferFromEntity<Waypoint>(true);

        Entities
			.ForEach((ref BodyInput bodyInput, ref PathMovement pathMovement, ref Rotation rotation, in Translation translation) =>
			{
				DynamicBuffer<float3> waypoints = allWaypointBuffers[pathMovement.Path].Reinterpret<float3>();
				float3 nextPosition = waypoints[pathMovement.CurrentTargetWaypoint];

				//check if the waypoint has been reached
				float distanceSq = math.lengthsq(translation.Value.xz - nextPosition.xz);
				if(distanceSq < .2f)
				{
					//waypoint reached
					if(pathMovement.CurrentTargetWaypoint == waypoints.Length-1)
					{
						if(pathMovement.IsLooping)
						{
							pathMovement.CurrentTargetWaypoint = 0;
						}
						else
						{
							pathMovement.CurrentTargetWaypoint--;
						}
					}
					else
					{
						pathMovement.CurrentTargetWaypoint++;
					}

					nextPosition = waypoints[pathMovement.CurrentTargetWaypoint];

					//instantly face next waypoint
					//TODO: slowly rotate to face the waypoint (not writing to Rotation, but passing the correct BodyInput.Movement.x value)
					float3 dir = math.normalize(nextPosition - translation.Value);
					rotation.Value = quaternion.LookRotation(dir, math.up());

					bodyInput.Movement = new float2(0f, .5f); //slow down
				}
				else
				{
					bodyInput.Movement = new float2(0f, math.clamp(bodyInput.Movement.y + 0.05f, 0f, 1f)); //accelerate
				}

			}).Schedule();
    }
}