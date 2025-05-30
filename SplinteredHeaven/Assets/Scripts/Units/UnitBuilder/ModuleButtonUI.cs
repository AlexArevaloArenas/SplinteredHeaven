using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleButtonUI : MonoBehaviour, IPointerClickHandler
{
    public Image unitImage;
    public TMPro.TMP_Text partText;
    public GameObject background;
    public UnitBuilderUI UnitBuilderUI;
    public ModuleData module;
    public ModuleSlot slot;

    private void Start()
    {
        background.SetActive(false); // Hide the background at the start
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClick code goes here ...
        //Use this to tell when the user left-clicks on the Button
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            background.SetActive(true);
            UnitBuilderUI.ChangeModulePart(module, slot);

        }
    }
    
    public void SetUp(ModuleData associatedModule, UnitBuilderUI uiController, ModuleSlot newSlot)
    {
        this.UnitBuilderUI = uiController;
        if (associatedModule == null)
        {
            module = null;
            partText.text = "Empty Slot";
            slot = newSlot;
        }
        else
        {
            if (associatedModule != null)
            {
                //unitImage.sprite = part.Image;
                module = associatedModule;
                partText.text = associatedModule.id;
                slot = newSlot;
                // Add more setup code here if needed
            }
            else
            {
                Debug.LogError($"Unit data for {associatedModule.name} not found.");
            }
        }
            
    }

}
