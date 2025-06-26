using UnityEngine;

[CreateAssetMenu(fileName = "LaserWeaponData", menuName = "Modules/LaserWeaponData")]
public class LaserWeaponData : WeaponData
{
    public LayerMask hitMask;
    public float inaccuracyAngle = 0f; // Optional spread
    public GameObject attackLaser; // Optional prefab for visual representation

    private GameObject laserInstance; // Instance of the laser prefab

    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart fallbackPart, Transform attackPoint)
    {
        if (laserInstance != null)
        {
            return; // Prevent multiple instances
        }
        Transform origin = attackPoint != null ? attackPoint : user.transform;
        Vector3 direction = GetInaccurateDirection(fallbackPart.transform.position-origin.position, inaccuracyAngle);


        laserInstance = Instantiate(attackLaser, attackPoint.position, Quaternion.LookRotation(direction), attackPoint);
        laserInstance.GetComponent<LaserComponent>().Init(user, damage,target.transform,fallbackPart.transform);

        /*
        if (Physics.Raycast(origin.position, direction, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            //UnitPart hitPart = hit.collider.GetComponentInParent<UnitPart>();
            DamageReceiver hitPart = hit.collider.GetComponent<DamageReceiver>();

            if (hitPart != null)
            {
                Debug.Log($"[Raycast Weapon] {user.name} hits {hitPart.name} for {damage} damage.");
                hitPart.ApplyDamage(damage, user.unit);

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
        */
    }

    private Vector3 GetInaccurateDirection(Vector3 forward, float angleDegrees)
    {
        if (angleDegrees <= 0f) return forward;

        float angleRad = angleDegrees * Mathf.Deg2Rad;
        Vector2 spread = Random.insideUnitCircle * Mathf.Tan(angleRad);
        Vector3 inaccurate = new Vector3(spread.x, spread.y, 1);
        return (Quaternion.LookRotation(forward) * inaccurate).normalized;
    }

    private void OnEnable()
    {
        laserInstance = null; // Reset the laser instance when the script is enabled    
    }
}
