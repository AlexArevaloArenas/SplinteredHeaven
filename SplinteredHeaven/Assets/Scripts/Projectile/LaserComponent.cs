using System.Collections;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{
    public UnitManager unit; // Reference to the owner of the missile
    public float launchForce = 20f;
    public GameObject destroyParticles; // Optional particle effect on destroy
    public float damage = 20f; // Damage value to apply on impact

    public float damageRatio = 0.2f;

    public Transform target; // Optional target for the missile to home in on
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(UnitManager _unit, float _damage, Transform _target)
    {
        unit = _unit;
        damage = _damage;
        target = _target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root == unit.transform.root)
        {
            // Ignore collisions with the same root object (self-collision)
            return;
        }
        // Check if the missile hits a target
        DamageReceiver damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            StartCoroutine(Damage(damageReceiver));
        }

    }

    private void OnDestroy()
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }

    public IEnumerator Damage(DamageReceiver damageR)
    {
        damageR.ApplyDamage(damage, unit.unit);
        yield return new WaitForSeconds(damageRatio); // Optional delay for visual effects
    }

}
