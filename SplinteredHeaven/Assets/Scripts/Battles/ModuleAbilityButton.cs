using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleAbilityButton : MonoBehaviour, IPointerClickHandler //, IPointerEnterHandler, IPointerExitHandler
{

    public ModuleInstance module;
    public UnitManager unit;
    public Sprite icon; // Icon for the button, can be set in the inspector or dynamically
    public string buttonText; // Text for the button, can be set in the inspector or dynamically

    public Image iconComponent; // Reference to the UI Image component for the icon
    public TMPro.TextMeshProUGUI textComponent; // Reference to the UI Text component for the button text

    //COOLDOWN
    public Image cooldownMask;
    public bool cooldownWaiting = false;

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (module == null) return;
        if (module.Data == null) return;
        if (module.Data.description == null) return;
        TooltipManager.Instance.ShowTooltip(module.Data.description, module.Data.icon);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
    */

    private void Start()
    {
        cooldownMask.fillAmount = 0; // Initialize cooldown mask to 0
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(module.Data.effectType == ModuleEffectType.Target)
        {
            
            //StartCoroutine(SearchingTarget());
        }
        else
        {
            if (cooldownWaiting) return;
            module.Data.ApplyEffects(unit, null,null,unit.transform);
            cooldownWaiting = true;
            cooldownMask.fillAmount = 1;
            StartCoroutine(Cooldown(module.Data.cooldown));
        }
    }

    public void Init(ModuleInstance _module, UnitManager _unit, Sprite newSprite, string text)
    {
        module = _module;
        unit = _unit;
        icon = newSprite;
        buttonText = text;

        iconComponent.sprite = icon; // Set the icon sprite
        textComponent.text = buttonText; // Set the button text
        
        if(module.Data.CurrentCooldown <= module.Data.cooldown)
        {
            cooldownWaiting = true; // Set cooldown waiting to true
            cooldownMask.fillAmount = module.Data.CurrentCooldown / module.Data.cooldown; // Set the cooldown mask fill amount
            StartCoroutine(Cooldown(module.Data.CurrentCooldown));
        }

    }
    /*
    public IEnumerator SearchingTarget()
    {
        module.Data.ApplyEffects(unit,, ,unit.transform);
    }
    */

    public IEnumerator Cooldown(float time)
    {
        float timer = time;
        while (timer >0)
        {
            timer = timer-Time.deltaTime;
            cooldownMask.fillAmount = (module.Data.CurrentCooldown / module.Data.cooldown);
            yield return null; // Wait for the next frame
        }
        cooldownWaiting = false;
    }

}
