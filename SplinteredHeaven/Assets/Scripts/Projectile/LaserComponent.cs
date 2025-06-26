using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LaserComponent : MonoBehaviour
{
    public UnitManager unit; // Reference to the owner of the missile
    public float launchForce = 20f;
    public GameObject destroyParticles; // Optional particle effect on destroy
    public float damage = 20f; // Damage value to apply on impact

    public float damageRatio = 0.1f;
    public float waitTime = 1f;

    public Transform target; // Optional target for the missile to home in on
    public Transform part; // Optional target for the missile to home in on
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private float timer = 0f;
    private float timerMax = 0.2f;
    public void Init(UnitManager _unit, float _damage, Transform _target, Transform _part)
    {
        unit = _unit;
        damage = _damage;
        target = _target;
        part = _part;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(part.transform.position - transform.position), 0.1f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (target == null)
        {
            DestroyLaser(1f);
        }

        if (timer < timerMax)
        {
            timer += Time.deltaTime;
            return;
        }

        if (collision.transform.root == unit.transform.root)
        {
            // Ignore collisions with the same root object (self-collision)
            return;
        }
        // Check if the missile hits a target
        DamageReceiver damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {

            damageReceiver.ApplyDamage(damage, unit.unit);
            StartCoroutine(DestroyLaser(waitTime));
        }

        timer = 0;

    }

    private void OnDestroy()
    {
        //Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }

    public IEnumerator Damage(DamageReceiver damageR)
    {
        yield return new WaitForSeconds(damageRatio); // Optional delay for visual effects

    }

    public IEnumerator DestroyLaser(float waitTime)
    {
        Material mat = transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material;
        while (mat.color.a > 0f)
        {
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, mat.color.a - 0.1f);
            mat.color = newColor;
            transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material = mat;
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(gameObject); // Destroy the missile object
    }

}
