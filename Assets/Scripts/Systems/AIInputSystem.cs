using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class AIInputSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities.ForEach((ref Inputs aiInputs) =>
		{
			//TODO: figure out the inputs for AI tanks
		}).Schedule();
	}
}