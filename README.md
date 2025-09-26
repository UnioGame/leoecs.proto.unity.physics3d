<p align="center">
    <img src="./logo.png" alt="Proto">
</p>

# LeoECS Proto – Unity Physics3D events integration
Integration of Unity Physics3D events for LeoECS Proto.

> IMPORTANT! Requires C# 9 (or Unity >= 2021.2).

> IMPORTANT! Depends on: [Leopotam.EcsProto](https://gitverse.ru/leopotam/ecsproto), [Leopotam.EcsProto.QoL](https://gitverse.ru/leopotam/ecsproto-qol), [Leopotam.EcsProto.Unity](https://gitverse.ru/leopotam/ecsproto.unity).

> IMPORTANT! Use `DEBUG` builds for development and `RELEASE` builds for production: all internal checks/exceptions work only in `DEBUG` builds and are removed in `RELEASE` builds to maximize performance.

> IMPORTANT! Tested on Unity 2021.3 (depends on it) and includes asmdef definitions to compile into separate assemblies and reduce main project recompilation time.


# Social
Official blog: https://leopotam.ru


# Installation

## As a Unity package
Installable as a Unity package via a Git URL in the Package Manager or by editing `Packages/manifest.json` directly:
```
"ru.leopotam.ecsproto.unity.physics3d": "https://gitverse.ru/leopotam/ecsproto.unity.physics3d.git",
```

## As source code
You can clone the repository or download an archive from the releases page.

## Other sources
The official, working version is hosted at [https://gitverse.ru/leopotam/ecsproto.unity.physics3d](https://gitverse.ru/leopotam/ecsproto.unity.physics3d). All other sources (including *nuget*, *npm*, and other repositories) are unofficial clones or third‑party code with unknown content.


# Unity Physics3D module
The module consumes Unity physics events and registers them as components. It consists of 2 parts: the aspect and the systems logic.


## Manual aspect wiring
Add the physics3d integration aspect to the main world aspect:
```csharp
class Aspect1 : ProtoAspectInject {
    public readonly UnityPhysics3DAspect Physics3D;
    // Register other aspects here.
}
```

## Manual systems wiring
```csharp
// Environment initialization.
using Leopotam.EcsProto.Unity;
using Leopotam.EcsProto.Unity.Ugui;

IProtoSystems _systems;

void Start () {
    _systems = new ProtoSystems (new ProtoWorld (new Aspect1 ()));
    _systems
        // Attach injection module.
        .AddModule (new AutoInjectModule ())
        // Attach Unity integration module.
        .AddModule (new UnityModule ())
        .Add (new TestSystem1 ())
        // Attach physics3d integration module.
        .AddModule (new UnityPhysics3DModule ())
        .Init ();
}
```

> IMPORTANT! Pay close attention to which systems group the module is added to. In the vast majority of cases, it should be the systems group that runs during FixedUpdate().

The physics3d module should be added where processed physics events will be cleaned up. To separate where events are registered from where they are removed, you can use a named order point and pass it to the constructor:
```csharp
// Environment initialization.
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using Leopotam.EcsProto.Unity.Physics3D;

IProtoSystems _systems;

void Start () {
    const int OrderPhysics3DClearPoint = 123;
    _systems = new ProtoSystems (new ProtoWorld (new Aspect1 ()));
    _systems
        // Attach injection module.
        .AddModule (new AutoInjectModule ())
        // Attach Unity integration module.
        .AddModule (new UnityModule ())
        // Attach physics3d integration module for the default world
        // with cleanup at the order point with weight OrderPhysics3DClearPoint.
        .AddModule (new UnityPhysics3DModule (default, OrderPhysics3DClearPoint))
        .Add (new TestSystem1 ())
        .Init ();
}
```

## Automatic wiring of the whole module
```csharp
// Environment initialization.
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using Leopotam.EcsProto.Unity.Physics3D;

IProtoSystems _systems;

void Start () {
    const int OrderPhysics3DClearPoint = 123;
    ProtoModules modules = new ProtoModules (
        new AutoInjectModule (),
        new UnityModule (),
        new UnityPhysics3DModule (default, OrderPhysics3DClearPoint)
    );
    _systems = new ProtoSystems (new ProtoWorld (modules.BuildAspect ()));
    _systems
        // Attach composite module.
        .AddModule (modules.BuildModule ())
        .Add (new TestSystem1 ())
        .Init ();
}
```

# Events
All supported event types are exposed by the physics3d aspect:
```csharp
public sealed class UnityPhysics3DAspect : ProtoAspectInject {
    public readonly ProtoPool<UnityPhysics3DCollisionEnterEvent> CollisionEnterEvent;
    public readonly ProtoPool<UnityPhysics3DCollisionExitEvent> CollisionExitEvent;
    public readonly ProtoPool<UnityPhysics3DControllerColliderHitEvent> ControllerColliderHitEvent;
    public readonly ProtoPool<UnityPhysics3DTriggerEnterEvent> TriggerEnterEvent;
    public readonly ProtoPool<UnityPhysics3DTriggerExitEvent> TriggerExitEvent;
}
```
You can build an iterator over any of them to handle reactions to events:
```csharp
using Leopotam.EcsProto.Unity.Physics3D;

public class TestTriggerEnterEventSystem : IProtoRunSystem {
    [DI] UnityPhysics3DAspect _phys3DEvents;
    [DI] ProtoIt _triggerEnterIt = new (It.Inc<UnityPhysics3DTriggerEnterEvent> ());

    public void Run () {
        foreach (ProtoEntity entity in _triggerEnterIt) {
            ref UnityPhysics3DTriggerEnterEvent data = ref _phys3DEvents.TriggerEnterEvent.Get (entity);
            Debug.Log ("enter to trigger!", data.Sender);
        }
    }
}
```


# Actions
Actions (`xxxAction` classes) are `MonoBehaviour` components that listen to Unity physics events and generate the corresponding ECS events:
* UnityPhysics3DCollisionEnterAction
* UnityPhysics3DCollisionExitAction
* UnityPhysics3DControllerColliderHitAction
* UnityPhysics3DTriggerEnterAction
* UnityPhysics3DTriggerExitAction


# License
This package is released under the MIT-ZARYA license, see [details here](./LICENSE.md).
