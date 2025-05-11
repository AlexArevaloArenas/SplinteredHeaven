using UnityEngine;

[CreateAssetMenu(fileName = "RaycastWeaponData", menuName = "Scriptable Objects/RaycastWeaponData")]
public class RaycastWeaponData : WeaponData
{
    public LayerMask hitMask;
    public float inaccuracyAngle = 0f; // Optional spread

    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart fallbackPart, Transform attackPoint)
    {
        Transform origin = attackPoint != null ? attackPoint : user.transform;
        Vector3 direction = GetInaccurateDirection(origin.forward, inaccuracyAngle);

        // ?? Draw the ray in the scene view for debugging
        Debug.DrawRay(origin.position, direction * range, Color.red, 1.5f);

        if (Physics.Raycast(origin.position, direction, out RaycastHit hit, range, hitMask))
        {
            UnitPart hitPart = hit.collider.GetComponentInParent<UnitPart>();

            if (hitPart != null && hitPart.currentHealth > 0)
            {
                Debug.Log($"[Raycast Weapon] {user.name} hits {hitPart.name} for {damage} damage.");
                hitPart.TakeDamage(damage);

                // Optional: draw impact point
                Debug.DrawLine(hit.point, hit.point + hit.normal * 0.5f, Color.yellow, 1.5f);
            }
            else
            {
                Debug.Log($"[Raycast Weapon] Hit object but no valid UnitPart. Hit: {hit.collider.name}");
            }
        }
        else
        {
            Debug.Log($"[Raycast Weapon] {user.name} missed (Raycast hit nothing).");
        }
    }

    private Vector3 GetInaccurateDirection(Vector3 forward, float angleDegrees)
    {
        if (angleDegrees <= 0f) return forward;

        float angleRad = angleDegrees * Mathf.Deg2Rad;
        Vector2 spread = Random.insideUnitCircle * Mathf.Tan(angleRad);
        Vector3 inaccurate = new Vector3(spread.x, spread.y, 1);
        return (Quaternion.LookRotation(forward) * inaccurate).normalized;
    }
}
