using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInput : IComponentData
{
	public float2 Movement;
	public float2 Pointer;
	public bool Fire;
}

public struct Velocity : IComponentData
{
    public float2 Value;
}

public struct BodyRotation : IComponentData
{
    public int Value;
}

public struct TurretRotation : IComponentData
{
    public int Value;
}

public struct Speed : IComponentData
{
    public float Value;
}

public struct Projectile : IComponentData
{
    public Entity Reference;
}