using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{

    [SerializeField] private GameObject UnitInfo;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private Image unitPicture;

    [SerializeField] private GameObject[] overlaySelectionList = new GameObject[10];

    private static Overlay _instance;
    public static Overlay Instance { get { return _instance; } }

    void Awake()
    {
        //If an instance of this already exists and it isn't this one
        if (_instance != null && _instance != this)
        {
            //We destroy this instance
            Destroy(this.gameObject);
        }
        else
        {
            //Make this the instance
            _instance = this;
        }
    }

    public void AddUnitInfo(UnitManager unitToShow)
    {

        unitPicture.sprite = unitToShow.unitData.Image;
        unitName.text = unitToShow.unitData.id;
    }

    public void HideOverlay()
    {
        UnitInfo.SetActive(false);
    }

    public void ShowOverlay()
    {
        UnitInfo.SetActive(true);
    }

    public void ShowUnitSelections()
    {
        int i = 0;
        foreach (var unit in UnitSelections.Instance.unitSelected)
        {
            overlaySelectionList[i].SetActive(true);
            i++;
        }
        SelectButton(0);
        ShowOverlay();
    }
    public void HideUnitSelections()
    {
        foreach (var item in overlaySelectionList)
        {
            item.SetActive(false);
        }
    }

    public void SelectButton(int n)
    {
        if (UnitSelections.Instance.unitSelected.Count>0)
        {
            AddUnitInfo(UnitSelections.Instance.unitSelected[n].GetComponent<UnitManager>());
        }
        
    }
}
