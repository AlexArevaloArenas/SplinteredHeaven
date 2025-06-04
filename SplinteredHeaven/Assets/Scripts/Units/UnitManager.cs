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
    public UnitData unitData;
    [SerializeField]
    [OnChangedCall("UpdateVisual")] public MechaClass mechaClass;

    /*
    [SerializeField]
    [OnChangedCall("UpdateVisual")]
    */

    //public TextAsset unitJsonData; // JSON data for the unit, used for loading and saving

    [SerializeField] public Unit unit;
    [SerializeField] RuntimeAssetRegistry registry; // Registry to access part and module data

    [Header("Selection Info")]
    public bool selected = false;
    public GameObject selector;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] public Image hpBar;

    private void Awake()
    {
        if (mechaClass != MechaClass.empty)
        {
            UpdateAll();
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
        EventManager.Instance.JumpEvent += ShieldTest;
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
        UnitRuntimeData runtimeData = MechaBuilder.LoadFromJson(mechaClass.ToString());
        unit = MechaBuilder.CreateUnitFromRuntimeData(runtimeData, registry, gameObject);
        unit.obj = gameObject;
        if (unitData == null) return;
        if (GetComponent<UnitVisualManager>() == null)
        {
            gameObject.AddComponent<UnitVisualManager>();
        }
        if (mechaClass == MechaClass.empty)
        {
            GetComponent<UnitVisualManager>().UnitData = unitData;

        }
    }

    public void UpdateAll()
    {
        UnitRuntimeData runtimeData = MechaBuilder.LoadFromJson(mechaClass.ToString());
        unit = MechaBuilder.CreateUnitFromRuntimeData(runtimeData, registry, gameObject);
        unit.obj = gameObject;
        //unit.InitHealthBar(hpBar);
        GetComponent<UnitVisualManager>().Init(unit);
    }

    public void ShieldTest()
    {
        if (selected)
        {
            Debug.Log("Escape pressed");
            foreach (ModuleInstance module in unit.Parts.SelectMany(p => p.Modules))
            {
                if (module.Data is EnergyShieldModuleData shieldModule)
                {
                    shieldModule.ApplyEffects(this, null, null, null);
                }
            }
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
    mecha03
}