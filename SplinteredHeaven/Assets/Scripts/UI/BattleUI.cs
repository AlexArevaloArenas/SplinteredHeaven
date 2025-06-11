using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    List<GameObject> unitsSelected;
    public GameObject unitInfoPanel;

    public GameObject unitInfoPanelPrefab;

    //Minimap
    public GameObject minimapPanel;
    public GameObject minimapPrefab;

    private void Start()
    {
        Instantiate(minimapPrefab, minimapPanel.transform);
    }
    public void ReloadUI()
    {
        unitsSelected = GetComponent<UnitSelections>()?.unitSelected;

    }
}
