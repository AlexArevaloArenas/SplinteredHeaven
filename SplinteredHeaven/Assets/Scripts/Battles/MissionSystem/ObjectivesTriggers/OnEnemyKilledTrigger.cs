using UnityEngine;

public class OnEnemyKilledTrigger : MonoBehaviour
{
    private void OnDestroy()
    {
        // Check if the object being destroyed is an enemy
        if (gameObject.CompareTag("Enemy"))
        {
            // Trigger the objective for killing an enemy
            MissionEvents.OnEnemyKilled?.Invoke(gameObject.name);
        }
    }
}
