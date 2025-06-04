using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public UnitPart part;

    public void Initialize(UnitPart part)
    {
        this.part = part;
    }

    public void ApplyDamage(float damage, Unit source)
    {
        part?.TakeDamage(damage);
    }
}
