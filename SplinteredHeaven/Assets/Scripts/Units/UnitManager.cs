using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;

//[RequireComponent(typeof(SelectableUnit))]
//[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(UnitVisualManager))]
public class UnitManager : MonoBehaviour //Unit Stores the Actions of the Unit
{
    [SerializeField]
    [OnChangedCall("UpdateVisual")]
    public UnitData unitData;
    [SerializeField] public Unit unit;

    [Header("Selection Info")]
    public bool selected = false;
    public GameObject selector;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] public Image hpBar;

    private void Awake()
    {
        if (unit == null)
        {
            //unit = new Unit(unitData, gameObject);

        }
        if (unitData == null) unitData = GetComponent<UnitVisualManager>().UnitData;
    }
    protected void Start()
    {
        hpBar = Instantiate(hpBarPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<Image>();
        hpBar.GetComponent<HealthBarVisual>().unidad = this;
        unit.InitHealthBar(hpBar);

        selector.SetActive(false);
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        foreach (var part in unit.Parts)
        {
            part.TickModules(dt);
        }
    }


    public virtual void SelectedActions()
    {
        selector.SetActive(true);
    }

    public virtual void UnselectedActions()
    {
        //base.UnselectedActions();
        selector.SetActive(false);
    }

    void OnDestroy()
    {
        if (selected) Selected(false);
        UnitSelections.Instance.unitList.Remove(this.gameObject);
        //Destroy(hpBar.gameObject);
    }

    public void Selected(bool s)
    {
        selected = s;
        if (selected)
        {
            SelectedActions();
        }
        else
        {
            UnselectedActions();
        }
    }
    public bool GetSelected()
    {
        return selected;
    }

    public void UpdateVisual()
    {
        GetComponent<UnitVisualManager>().UnitData = unitData;
    }

}

public enum Faction
{
    Player,
    Enemy,
    Neutral
}