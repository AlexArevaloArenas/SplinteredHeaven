using System;
using UnityEngine;

namespace UtilityAI {
    public class DrawGizmo : MonoBehaviour {
        public Color color = Color.yellow;
        public float radius = 1f;
        
        [SerializeField] new SphereCollider collider;
        
        void Start() {
            if (collider == null) collider = GetComponent<SphereCollider>();
            radius = collider.radius;
        }

        void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}