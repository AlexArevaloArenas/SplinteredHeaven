
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ModuleSlotUI : MonoBehaviour, IPointerClickHandler
{
    public ModuleData currentModule; // The current module assigned to this slot
    public ModuleSlot slot; // The ModuleSlot this UI represents

    public GameObject background; // The background GameObject to show/hide

    public TMPro.TMP_Text moduleSlotText; // Text to display the module's name or ID

    public ModulePositionType positionType; // The type of position this slot can hold
    public ModuleWeightType weightType; // The type of weight this slot can hold
    private RectTransform rectTransform; // RectTransform for positioning

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        background.SetActive(false); // Initially hide the background
    }

    private void Update()
    {
        if (slot == null)
        {
            Destroy(gameObject); // Destroy this UI if the slot is not set
        }
        Vector3 newPos = Camera.main.WorldToScreenPoint(slot.transform.position);
        rectTransform.position = new Vector3(newPos.x, newPos.y, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClick code goes here ...
        //Use this to tell when the user left-clicks on the Button
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UnitBuilderUI.instance.HideSlotbackgrounds();
            background.SetActive(true);
            UnitBuilderUI.instance.ShowAvailableModules(currentModule, slot.modulePositionType, slot.moduleWeightType, slot); // Show available modules for this slot
        }
    }

    public void SetUp(ModuleSlot newSlot)
    {
        slot = newSlot;
        
    }
}
