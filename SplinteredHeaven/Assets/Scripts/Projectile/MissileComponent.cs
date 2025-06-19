using UnityEngine;

public class MissileComponent : MonoBehaviour
{
    public UnitManager unit; // Reference to the owner of the missile
    public float launchForce = 20f;
    public GameObject destroyParticles; // Optional particle effect on destroy
    public float damage = 20f; // Damage value to apply on impact
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Apply force to the rocket upon launch
        GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }

    public void Init(UnitManager _unit, float _damage)
    {
        unit = _unit;
        damage = _damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root == unit.transform.root)
        {
            // Ignore collisions with the same root object (self-collision)
            return;
        }
        // Check if the missile hits a target
        DamageReceiver damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            // Apply damage to the target
            damageReceiver.ApplyDamage(damage, null); // Replace 100 with actual damage value
            Debug.Log($"Missile hit {collision.gameObject.name} and applied damage.");
        }
        else
        {
            Debug.Log($"Missile hit {collision.gameObject.name} but no DamageReceiver found.");
        }
        // Destroy the missile after impact
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }
}
