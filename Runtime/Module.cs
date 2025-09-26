// ----------------------------------------------------------------------------
// Лицензия MIT-ZARYA
// (c) 2025 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System;
using Leopotam.EcsProto.QoL;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsProto.Unity.Physics3D {
    public sealed class UnityPhysics3DModule : IProtoModule {
        readonly string _worldName;
        readonly int _clearPointWeight;

        public UnityPhysics3DModule (string worldName = default, int clearPointWeight = default) {
            _worldName = worldName;
            _clearPointWeight = clearPointWeight;
        }

        public void Init (IProtoSystems systems) {
            systems
                .DelHere<UnityPhysics3DCollisionEnterEvent> (_worldName, _clearPointWeight)
                .DelHere<UnityPhysics3DCollisionExitEvent> (_worldName, _clearPointWeight)
                .DelHere<UnityPhysics3DControllerColliderHitEvent> (_worldName, _clearPointWeight)
                .DelHere<UnityPhysics3DTriggerEnterEvent> (_worldName, _clearPointWeight)
                .DelHere<UnityPhysics3DTriggerExitEvent> (_worldName, _clearPointWeight);
        }

        public IProtoAspect[] Aspects () {
            return _worldName != null ? null : new IProtoAspect[] { new UnityPhysics3DAspect () };
        }

        public Type[] Dependencies () => new Type[] { typeof (UnityModule) };
    }

    public sealed class UnityPhysics3DAspect : ProtoAspectInject {
        public readonly ProtoPool<UnityPhysics3DCollisionEnterEvent> CollisionEnterEvent;
        public readonly ProtoPool<UnityPhysics3DCollisionExitEvent> CollisionExitEvent;
        public readonly ProtoPool<UnityPhysics3DControllerColliderHitEvent> ControllerColliderHitEvent;
        public readonly ProtoPool<UnityPhysics3DTriggerEnterEvent> TriggerEnterEvent;
        public readonly ProtoPool<UnityPhysics3DTriggerExitEvent> TriggerExitEvent;
    }
}

#if ENABLE_IL2CPP
// Unity IL2CPP performance optimization attribute.
namespace Unity.IL2CPP.CompilerServices {
    enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2
    }

    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; private set; }
        public object Value { get; private set; }

        public Il2CppSetOptionAttribute (Option option, object value) { Option = option; Value = value; }
    }
}
#endif
