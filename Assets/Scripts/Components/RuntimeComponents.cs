using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct Inputs : IComponentData
{
	public float2 Movement;
	public float2 Pointer;
	public bool Fire;
}

public struct Velocity : IComponentData
{
    public float Value;
}

public struct Speed : IComponentData
{
    public float Value;
}

public struct FireCooldown : IComponentData
{
    public float Value;
}

public struct FireInterval : IComponentData
{
    public float Value;
}

public struct Projectile : IComponentData
{
    public Entity Reference;
}