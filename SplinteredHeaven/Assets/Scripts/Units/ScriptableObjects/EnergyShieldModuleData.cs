using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyShieldData", menuName = "Modules/EnergyShield")]
public class EnergyShieldModuleData : ModuleData
{
    public float shieldStrength = 100f; // Total shield strength
    public float duration = 5f; // Duration of the shield effect
    public Material shieldMaterial; // Material to apply for the shield visuals
    public DamageModifier damageModifier; // Damage modifier to apply when the shield is hit
    public override void ApplyEffects(UnitManager user, UnitManager target, UnitPart part, Transform attackPoint)
    {
        foreach (Transform child in user.GetComponentsInChildren<Transform>(true))
        {
            if (child.transform.GetComponent<SkinnedMeshRenderer>() == null) continue; // Skip if no MeshRenderer
            if (child.transform.GetComponent<SkinnedMeshRenderer>().materials == null) {

                child.transform.GetComponent<SkinnedMeshRenderer>().materials = new Material[0]; // Ensure materials array is initialized
                child.transform.GetComponent<SkinnedMeshRenderer>().materials[0] = shieldMaterial; // Add shield materials to the part
            }
            else
            {
                List<Material> newMaterials = child.transform.GetComponent<SkinnedMeshRenderer>().materials.ToList(); // Get all materials of the child
                newMaterials.Add(shieldMaterial); // Add shield material to the list

                child.transform.GetComponent<SkinnedMeshRenderer>().materials = newMaterials.ToArray(); // Set the modified materials back to the MeshRenderer
            }
                
        }
        user.StartCoroutine(ApplyShield(user, part)); // Start the coroutine to apply the shield
    }

    public IEnumerator ApplyShield(UnitManager user, UnitPart part)
    {
        yield return new WaitForSeconds(duration); // Wait for 1 second
        if (user.unit.damgMods.Contains(damageModifier))
        {
            user.unit.damgMods.Remove(damageModifier); // Remove damage modifier if it exists
            foreach (Transform child in user.GetComponentsInChildren<Transform>(true))
            {
                if (child.transform.GetComponent<MeshRenderer>().materials[2] != null)
                {
                    child.transform.GetComponent<MeshRenderer>().materials[2] = null; // Add shield materials to the part
                }
            }
            //if()

            /*
            // Simulate shield application
            float remainingStrength = shieldStrength;
            float elapsedTime = 0f;
            while (elapsedTime < duration && remainingStrength > 0f)
            {
                // Simulate shield effect over time
                yield return new WaitForSeconds(1f); // Wait for 1 second
                elapsedTime += 1f;
                remainingStrength -= 10f; // Decrease shield strength over time
                Debug.Log($"[Energy Shield] {user.name}'s shield on {part.name} has {remainingStrength} strength remaining.");
            }
            if (remainingStrength <= 0f)
            {
                Debug.Log($"[Energy Shield] {user.name}'s shield on {part.name} has expired.");
            }
            */
        }

    }
}
