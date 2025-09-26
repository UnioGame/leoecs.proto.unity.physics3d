// ----------------------------------------------------------------------------
// Лицензия MIT-ZARYA
// (c) 2025 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Leopotam.EcsProto.Unity.Physics3D {
    public struct UnityPhysics3DCollisionExitEvent {
        public string SenderName;
        public GameObject Sender;
        public Collider Collider;
        public Vector3 Velocity;
    }
}
