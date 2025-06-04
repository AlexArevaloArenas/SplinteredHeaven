using UnityEngine;

public class AimingController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Transform aimTarget;               // The target to face
    public float rotationSpeed = 5f;          // Speed of rotation
    public bool constrainToY = false;          // Lock vertical axis for horizontal turning only
    public bool constrainToX = false;          // Lock vertical axis for horizontal turning only
    public bool constrainToZ = false;          // Lock vertical axis for horizontal turning only

    [Header("Rotation Constraints")]
    public bool constrainRotation = true;
    public Vector3 minRotation = new Vector3(-45, -90, -10);
    public Vector3 maxRotation = new Vector3(45, 90, 10);

    private Quaternion originalLocalRotation;

    void Start()
    {
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        //Debug.Log($"AimingController Update: aimTarget = {aimTarget}");
        if (aimTarget != null)
        {
            Vector3 direction = aimTarget.position - transform.position;
            if (constrainToY) direction.y = 0f;
            if (constrainToX) direction.x = 0f;
            if (constrainToZ) direction.z = 0f;

            if (direction.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion newWorldRot = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                // Convert to local rotation
                Quaternion localRot = transform.parent != null ? Quaternion.Inverse(transform.parent.rotation) * newWorldRot : newWorldRot;

                if (constrainRotation)
                {
                    Vector3 euler = localRot.eulerAngles;
                    euler.x = NormalizeAngle(euler.x);
                    euler.y = NormalizeAngle(euler.y);
                    euler.z = NormalizeAngle(euler.z);

                    euler.x = Mathf.Clamp(euler.x, minRotation.x, maxRotation.x);
                    euler.y = Mathf.Clamp(euler.y, minRotation.y, maxRotation.y);
                    euler.z = Mathf.Clamp(euler.z, minRotation.z, maxRotation.z);
                    transform.localRotation = Quaternion.Euler(euler);
                }
                else
                {
                    transform.localRotation = localRot;
                }
            }  
        }
        else
        {
            // Smoothly return to original rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originalLocalRotation, Time.deltaTime * rotationSpeed);
        }


        /*
           var targetPosition = aimTarget.position;
           targetPosition.y = transform.position.y;

           transform.LookAt(targetPosition);
           */

    }



    public void SetTarget(Transform target)
    {
        aimTarget = target;
    }

    public void ClearTarget()
    {
        aimTarget = null;
    }

    public bool HasTarget => aimTarget != null;

    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
