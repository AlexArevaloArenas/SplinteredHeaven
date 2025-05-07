using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public UnitPart part;

    public void ApplyDamage(float damage, Unit source)
    {
        part?.TakeDamage(damage);
    }
}
