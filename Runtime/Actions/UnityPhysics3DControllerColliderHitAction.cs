// ----------------------------------------------------------------------------
// Лицензия MIT-ZARYA
// (c) 2025 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsProto.Unity.Physics3D {
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public sealed class UnityPhysics3DControllerColliderHitAction : ProtoUnityAction<UnityPhysics3DControllerColliderHitEvent> {
        public void OnControllerColliderHit (ControllerColliderHit hit) {
            if (IsValidForEvent ()) {
                ref var msg = ref NewEvent ();
                msg.SenderName = SenderName ();
                msg.Sender = Sender ();
                msg.Collider = hit.collider;
                msg.Point = hit.point;
                msg.Normal = hit.normal;
            }
        }
    }
}
