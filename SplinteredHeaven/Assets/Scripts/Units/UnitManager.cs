using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[RequireComponent(typeof(SelectableUnit))]
//[RequireComponent(typeof(Agent))]
public abstract class UnitManager : MonoBehaviour //Unit Stores the Actions of the Unit
{
    public UnitData unitData;
    public Unit unit;

    [Header("Selection Info")]
    public bool selected = false;
    public GameObject selector;

    [Header("HP Bar")]
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] public Image hpBar;

    

    protected virtual void Start()
    {
        hpBar = Instantiate(hpBarPrefab, GameObject.FindWithTag("Canvas").transform).GetComponent<Image>();
        hpBar.GetComponent<HealthBarVisual>().unidad = this;
        Initialize();
        selector.SetActive(false);
        Debug.Log("Add unit");
        //gridAgent = GetComponent<Agent>();
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    public virtual void Initialize()
    {
        unit = new Unit(unitData, hpBar);   
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

    private void _Die()
    {
        if (selected)
            Selected(false);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
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

}

public enum Faction
{
    Player,
    Enemy,
    Neutral
}