using Unity.Entities;
using UnityEngine;
using Unity.Physics.Systems;
using Time = UnityEngine.Time;
 
//Temporary hack systems to fix the lack of FixedUpdate in ECS
//Credits to Ivan 'Nothke' Notaroš for the hack

//This system sets the fixedDeltaTime to the value of deltaTime so
//the ECS physics (which are using fixedDeltaTime anyway) can be in sync with the ECS loop.
[DisableAutoCreation]
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class PrePhysicsSetDeltaTimeSystem : SystemBase
{
    public bool isRealTimeStep = true; //Change this to false to disable the hack
    public float timeScale = 1;
    public float originalFixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
 
    protected override void OnUpdate()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
 
        if (isRealTimeStep
			&& UnityEngine.Time.frameCount > 5)
           UnityEngine.Time.fixedDeltaTime = UnityEngine.Time.deltaTime * timeScale;
        else
            UnityEngine.Time.fixedDeltaTime = Time.fixedDeltaTime * timeScale;
    }
}
 
//This system just puts the fixedDeltaTime back to its original value
[DisableAutoCreation]
[UpdateAfter(typeof(ExportPhysicsWorld))]
public class PostPhysicsResetDeltaTimeSystem : ComponentSystem
{
    public PrePhysicsSetDeltaTimeSystem preSystem;
 
    protected override void OnCreate()
    {
        preSystem = World.GetOrCreateSystem<PrePhysicsSetDeltaTimeSystem>();
    }
 
    protected override void OnUpdate()
    {
        UnityEngine.Time.fixedDeltaTime = preSystem.originalFixedDeltaTime;
    }
}