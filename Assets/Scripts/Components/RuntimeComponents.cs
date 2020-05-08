using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct BodyInput : IComponentData
{
	public float2 Movement;
}

public struct TurretInput : IComponentData
{
	public float2 Target;
	public bool Fire;
}

public struct Velocity : IComponentData
{
    public float Value;
}

public struct MovementSpeed : IComponentData
{
    public float Value;
}

public struct RotationSpeed : IComponentData
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

public struct FireSpeed : IComponentData
{
    public float Value;
}

public struct ProjectilePrefab : IComponentData
{
    public Entity Reference;
}

public struct ProjectileSpawnPoint : IComponentData
{
	public float3 LocalTranslation;
	public quaternion LocalRotation;
}

public struct PathMovement : IComponentData
{
	public Entity Path;
	public int CurrentTargetWaypoint;
	public bool IsLooping;
}

public struct Waypoint : IBufferElementData
{
	public float3 Position;
}