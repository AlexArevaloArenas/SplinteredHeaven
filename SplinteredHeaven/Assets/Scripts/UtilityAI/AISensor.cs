using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    [RequireComponent(typeof(SphereCollider))]
    public class AISensor : MonoBehaviour
    {
        public float detectionRadius = 10f;
        public List<string> targetTags = new();

        [SerializeField]
        public readonly List<Transform> detectedObjects = new(30);
        SphereCollider sphereCollider;

        void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = detectionRadius;

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var c in colliders)
            {
                ProcessTrigger(c, transform => detectedObjects.Add(transform));
            }
        }

        void OnTriggerEnter(Collider other)
        {
            ProcessTrigger(other, transform => detectedObjects.Add(transform));
        }

        void OnTriggerExit(Collider other)
        {
            ProcessTrigger(other, transform => detectedObjects.Remove(transform));
        }

        public virtual void ProcessTrigger(Collider other, Action<Transform> action)
        {
            if (other.CompareTag("Untagged")) return;

            foreach (string t in targetTags)
            {
                if (other.CompareTag(t))
                {
                    action(other.transform);
                }
            }
        }

        public Transform GetClosestTarget(string tag)
        {
            if (detectedObjects.Count == 0) return null;

            Transform closestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (Transform potentialTarget in detectedObjects)
            {
                if (potentialTarget.CompareTag(tag))
                {
                    Vector3 directionToTarget = potentialTarget.position - currentPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        closestTarget = potentialTarget;
                    }
                }
            }
            return closestTarget;
        }

        public void RefreshColliderData()
        {
            if (sphereCollider == null) return;
            sphereCollider.radius = detectionRadius;
            //Debug.Log($"Sensor radius updated to {detectionRadius}");
            detectedObjects.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var c in colliders)
            {
                ProcessTrigger(c, transform => detectedObjects.Add(transform));
            }
        }
    }
}