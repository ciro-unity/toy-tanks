using UnityEngine;
using Unity.Entities;

public class PathAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	public Vector3[] waypoints;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		DynamicBuffer<Waypoint> waypointBuffer = dstManager.AddBuffer<Waypoint>(entity);
		
		int i = 0;
		foreach(Vector3 wp in waypoints)
		{
			waypointBuffer.Add(new Waypoint{Position = waypoints[i]});
			i++;
		}
	}

	public void Reset()
	{
		waypoints = new Vector3[4];
		float defaultDistance = 5f;
		waypoints [0] = new Vector3 (0f, 0f, defaultDistance) + transform.position;
		waypoints [1] = new Vector3 (defaultDistance, 0f, 0f) + transform.position;
		waypoints [2] = new Vector3 (0f, 0f, -defaultDistance) + transform.position;
		waypoints [3] = new Vector3 (-defaultDistance, 0f, 0f) + transform.position;
	}
}
