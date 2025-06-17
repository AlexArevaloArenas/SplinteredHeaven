using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private LayerMask targetLayer;
        private SphereCollider sphereCollider;

        readonly List<Transform> detectedObjects = new(10);
        public void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = detectionRange;


        }
        public bool IsTargetInRange(Transform target)
        {
            if (target == null) return false;
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= detectionRange && IsTargetVisible(target);
        }
        private bool IsTargetVisible(Transform target)
        {
            RaycastHit hit;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionRange, targetLayer))
            {
                return hit.transform == target;
            }
            return false;
        }
    }
}
