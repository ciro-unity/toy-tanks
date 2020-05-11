using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

//------------------------------------   INPUT   ------------------------------------

public struct BodyInput : IComponentData
{
	public float2 Movement;
}

public struct TurretInput : IComponentData
{
	public float2 Target;
	public bool Fire;
}


//------------------------------------   MOVEMENT OF TANKS   ------------------------------------

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


//------------------------------------   FIRING MECHANICS   ------------------------------------

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

//------------------------------------   PATHS   ------------------------------------

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

//------------------------------------   PARTICLES   ------------------------------------

public struct EmitParticlesOnCreationTag : IComponentData { }
public struct EmitParticlesOnDestructionTag : IComponentData { }

public struct ParticleEffects : IComponentData
{
	public Entity CreationEffect;
	public Entity DestructionEffect;
}

public struct ParticleEffectPreset : IComponentData
{
	public Entity ParticlePrefab;
	public int NumberOfParticles;
	public float2 InitialSpeedRange;
	public float2 LifetimeRange;
}

public struct Particle : IComponentData
{
	public float Lifetime;
	public float ElapsedTime;
	public float3 MovementDirection;
	public float Velocity;
}

//------------------------------------   LIFECYCLE   ------------------------------------

public struct DestroyTag : IComponentData { }