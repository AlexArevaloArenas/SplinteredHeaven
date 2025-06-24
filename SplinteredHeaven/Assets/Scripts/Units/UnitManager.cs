using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEditor;
using Unity.AppUI.UI;
using System.Linq;

//[RequireComponent(typeof(SelectableUnit))]
//[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(UnitVisualManager))]
public class UnitManager : MonoBehaviour //Unit Stores the Actions of the Unit
{
    [OnChangedCall("EditorVisual")] public UnitData unitData;
    [SerializeField]
    [OnChangedCall("EditorVisual")] public MechaClass mechaClass;

    [SerializeField] public Unit unit;
    [SerializeField] RuntimeAssetRegistry registry; // Registry to access part and module data

    [Header("Selection Info")]
    public bool selected = false;
    public GameObject selector;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] public Image hpBar;

    public bool visible = true; // For Editor

    private void Awake()
    {
        if (mechaClass != MechaClass.empty)
        {
            UpdateAll();
        }
        else if (unitData != null)
        {
            unit = new Unit(unitData, gameObject);
            UpdateVisual();
        }
        /*
        else if (unit == null)
        {
            unit = new Unit(unitData, gameObject);
            UpdateVisual();
            /*
            Debug.Log("Unit is null, creating new instance.");
             
        }
        */
        //if (unitData == null) unitData = GetComponent<UnitVisualManager>().UnitData;
    }
    protected void Start()
    {
        if (hpBar == null)
        {
            hpBar = Instantiate(hpBarPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<Image>();
            hpBar.GetComponent<HealthBarVisual>().unidad = this;
            unit.InitHealthBar(hpBar);
        }

        if (selector != null)
        {
            selector.SetActive(false);
            UnitSelections.Instance.unitList.Add(this.gameObject);
        }

        GetComponent<TargetTracker>().detectionInterval = unit.detectionInterval; // Set detection interval from unit
        GetComponent<TargetTracker>().detectionRadius = unit.detectionRange;
        GetComponent<TargetTracker>().RefreshColliderData();

    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
        unit.obj = gameObject;
        if (hpBar != null)
        {
            hpBar = Instantiate(hpBarPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<Image>();
            hpBar.GetComponent<HealthBarVisual>().unidad = this;
            unit.InitHealthBar(hpBar);
        }
        GetComponent<UnitVisualManager>().Init(unit);
    }

    private void Update()
    {
        if(unit == null) return;
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
        if(UnitSelections.Instance == null) return;
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
        GetComponent<UnitVisualManager>().Init(unit);

    }

    public void UpdateAll()
    {
        UnitRuntimeData runtimeData = MechaBuilder.LoadFromJson(mechaClass.ToString());
        unit = MechaBuilder.CreateUnitFromRuntimeData(runtimeData, registry, gameObject);
        unit.obj = gameObject;
        //unit.InitHealthBar(hpBar);
        GetComponent<UnitVisualManager>().Init(unit);
    }

    public void EditorVisual()
    {
        if (mechaClass != MechaClass.empty)
        {
            UnitRuntimeData runtimeData = MechaBuilder.LoadFromJson(mechaClass.ToString());
            unit = MechaBuilder.CreateUnitFromRuntimeData(runtimeData, registry, gameObject);
            unit.obj = gameObject;
            GetComponent<UnitVisualManager>().Init(unit);
        }
        else
        {
            unit = new Unit(unitData, gameObject);
            GetComponent<UnitVisualManager>().Init(unit);
        }
    }

    public void SetHealthbarVisibility(bool visible)
    {
        if (hpBar != null)
        {
            hpBar.gameObject.SetActive(visible);
        }
    }

}

public enum Faction
{
    Player,
    Enemy,
    Neutral
}
public enum MechaClass
{
    empty,
    mecha01,
    mecha02,
    mecha03,
    tank01,
}