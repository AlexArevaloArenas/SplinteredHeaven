using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitButtonUI : MonoBehaviour, IPointerClickHandler
{
    public Image unitImage;
    public TMPro.TMP_Text partText;
    public GameObject background;
    public UnitBuilderUI UnitBuilderUI;
    public UnitPartData part;

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
            UnitBuilderUI.ChangeMechaPart(part);
        }
    }
    
    public void SetUp(UnitPartData associatedPart, UnitBuilderUI uiController)
    {
        this.UnitBuilderUI = uiController;
        if (associatedPart != null)
        {
            //unitImage.sprite = part.Image;
            part = associatedPart;
            partText.text = part.id;
            // Add more setup code here if needed
        }
        else
        {
            Debug.LogError($"Unit data for {part.name} not found.");
        }
    }

}
