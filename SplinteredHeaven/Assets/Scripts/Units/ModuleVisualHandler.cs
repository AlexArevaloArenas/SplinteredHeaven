using UnityEngine;
using static UnityEngine.UI.Image;

public class ModuleVisualHandler : MonoBehaviour
{
    public ModuleInstance linkedModule;
    public Animator animator;
    public ParticleSystem effectOnActivate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(ModuleInstance module)
    {
        Debug.Log("Initialize module");
        linkedModule = module;

        //gameObject.name = module.Data.moduleName + "_Visual";

        if (linkedModule is WeaponModuleInstance weapon)
        {
            //weapon.Activate += HandleFired;
            foreach (Transform child in transform.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "AttackPoint")
                {
                    Debug.Log($"[ModuleVisualHandler] Found AttackPoint for {linkedModule.Data.id}.");
                    linkedModule.origin = child;
                    break;
                }
            }
        }

        if (linkedModule.disabled)
        {
            SetDisabledVisuals();
        }
        
    }
    /*
    private void HandleFired()
    {
        Debug.Log($"[ModuleVisualHandler] {linkedModule.Data.moduleName} fired!");

        effectOnActivate?.Play();
        animator?.SetTrigger("Fire");
    }
    */
    private void SetDisabledVisuals()
    {
        if (animator) animator.SetBool("Disabled", true);
    }

    private void OnDestroy()
    {
        if (linkedModule is WeaponModuleInstance weapon)
        {
            //weapon.OnFired -= HandleFired;
        }
    }
}
