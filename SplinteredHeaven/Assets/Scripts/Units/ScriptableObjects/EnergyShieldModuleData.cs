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
        CurrentCooldown = cooldown; // Reset the cooldown timer
        user.StartCoroutine(Cooldown(cooldown)); // Start the coroutine to apply the shield
        user.StartCoroutine(ApplyShield(user, part)); // Start the coroutine to apply the shield
    }

    public IEnumerator ApplyShield(UnitManager user, UnitPart part)
    {
        foreach(var _part in user.unit.Parts)
        {
            if (_part.shield) continue; // Skip if the part already has a shield
            _part.shield = true; // Set the shield flag to true
        }
        yield return new WaitForSeconds(duration); // Wait
        foreach (Transform child in user.GetComponentsInChildren<Transform>(true))
        {
            if (child.transform.GetComponent<SkinnedMeshRenderer>() == null) continue; // Skip if no MeshRenderer
            List<Material> newMaterials = child.transform.GetComponent<SkinnedMeshRenderer>().materials.ToList(); // Get all materials of the child
            newMaterials.Remove(newMaterials.Last()); // Remove the shield material from the list
            child.transform.GetComponent<SkinnedMeshRenderer>().materials = newMaterials.ToArray(); // Set the modified materials back to the MeshRenderer
        }

        foreach (var _part in user.unit.Parts)
        {
            _part.shield = false; // Reset the shield flag
        }
        /*
        if (user.unit.damgMods.Contains(damageModifier))
        {
            user.unit.damgMods.Remove(damageModifier); // Remove damage modifier if it exists
           

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
        //}

    }

    public IEnumerator Cooldown(float time)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime; // Decrease the timer
            CurrentCooldown = timer; // Update the current cooldown
            yield return null; // Wait for the next frame
        }
        CurrentCooldown = 100000f; // Reset cooldown when done
    }
}
