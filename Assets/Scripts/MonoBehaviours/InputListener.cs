using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour, GameControls.IGameplayActions
{
	[HideInInspector] public Vector2 moveInput;
	[HideInInspector] public Vector2 mousePositionInput;
	[HideInInspector] public bool fireInput;

	private Plane groundPlane;
	private GameControls playerControls;

	private void Start()
	{
        groundPlane = new Plane(Vector3.up, Vector3.zero);
	}

	void OnEnable()
    {
		//set this GameObject as a listener for the events coming from the Input System
		playerControls = new GameControls();
		playerControls.Gameplay.SetCallbacks(this);
		playerControls.Enable();
    }

	void OnDisable()
    {
		playerControls.Disable();
    }

	//------------ Input reading ------------ //

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}

	public void OnAim(InputAction.CallbackContext context)
	{
		mousePositionInput = context.ReadValue<Vector2>();

		Ray ray = Camera.main.ScreenPointToRay(mousePositionInput);

		float enter = 0.0f;
		if (groundPlane.Raycast(ray, out enter))
		{
			Vector3 hitPoint = ray.GetPoint(enter);
			
			mousePositionInput.x = hitPoint.x;
			mousePositionInput.y = hitPoint.z;
		}
	}

	public void OnFire(InputAction.CallbackContext context)
	{
		fireInput = context.ReadValueAsButton();
	}
}
