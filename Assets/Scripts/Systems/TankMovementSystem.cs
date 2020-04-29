using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TankMovementSystem : SystemBase
{
	protected override void OnUpdate()
	{
		float deltaTime = Time.DeltaTime;

		Entities
		.ForEach((ref Velocity velocity, ref Translation translation, ref Rotation rotation, in Inputs inputs, in Speed targetSpeed) =>
		{
			//movement speed builds up in time, and is also reduced if the player lets go of the controls
			velocity.Value = math.min((velocity.Value * .9f) + inputs.Movement.y, targetSpeed.Value);
			translation.Value = translation.Value + (math.forward(rotation.Value) * velocity.Value * deltaTime);

			//fixed rotation value
			rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), inputs.Movement.x * 3f * deltaTime));
		}).Schedule();
	}
}