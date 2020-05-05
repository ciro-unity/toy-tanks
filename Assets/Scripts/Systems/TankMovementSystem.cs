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
		.ForEach((ref Velocity velocity, ref Translation translation, ref Rotation rotation, in BodyInput input, in MovementSpeed movementSpeed, in RotationSpeed rotationSpeed) =>
		{
			//movement speed builds up in time, and is also reduced if the player lets go of the controls
			velocity.Value = math.clamp((velocity.Value * .9f) + input.Movement.y, -movementSpeed.Value, movementSpeed.Value);
			translation.Value = translation.Value + (math.forward(rotation.Value) * velocity.Value * deltaTime);

			//fixed rotation value
			rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), input.Movement.x * rotationSpeed.Value * deltaTime));
		}).Schedule();
	}
}