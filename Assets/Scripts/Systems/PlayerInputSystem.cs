using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputSystem : SystemBase
{
	private InputListener input;

	protected override void OnCreate()
	{
		//retrieve the MonoBehaviour which acts as listener for events from the Input System
		input = GameObject.FindObjectOfType<InputListener>();
	}

	protected override void OnUpdate()
	{
		//transferring input to local variables
		float2 moveInput = input.moveInput;
		float2 mousePositionInput = input.mousePositionInput;
		bool fireInput = input.fireInput;

		Entities
		.WithAll<PlayerTag>()
		.ForEach((ref BodyInput playerInput) =>
		{
			playerInput.Movement = moveInput;
		}).Schedule();

		Entities
		.WithAll<PlayerTag>()
		.ForEach((ref TurretInput playerInput) =>
		{
			playerInput.Target = mousePositionInput;
			playerInput.Fire = fireInput;
		}).Schedule();
	}
}