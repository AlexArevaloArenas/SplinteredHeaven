using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnitBuilderUI : MonoBehaviour
{
    public Camera myCam;
    public LayerMask layerMask;

    public static UnitBuilderUI instance;
    public UnitManager UnitManager;
    public GameObject availablePartsPanel;
    public GameObject availableModulesPanel;
    public GameObject unitDetailsPanel;
    public TMP_Dropdown mechaDropdown;
    int currentIndex = 0;

    public GameObject buttonInstance;
    public GameObject moduleButtonInstance;
    public GameObject moduleSlotInstance;

    private List<GameObject> moduleSlots = new List<GameObject>();  
    private List<GameObject> availablePartsUI = new List<GameObject>();
    private List<GameObject> availableModulesUI = new List<GameObject>();

    [Header("Data")]
    public RuntimeAssetRegistry registry;

    public PartType selectedPart;

 
    private void Start()
    {
        
        myCam = Camera.main;
        EventManager.Instance.LeftMouseDownEvent += Click;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        SetUpAvailableUnits();
        RefreshMechaList();
    }
    /*
    public void SetUpAvailableUnits()
    {
        TextAsset[] unitData = Resources.LoadAll<TextAsset>("UnitJSON/");
        foreach (TextAsset unit in unitData)
        {
            Unit unitInfo = JsonUtility.FromJson<Unit>(unit.text);
            GameObject unitButton = Instantiate(availableUnitsPanel, availableUnitsPanel.transform.position, Quaternion.identity, availableUnitsPanel.transform);
        }
    }*/

    public void Click()
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        */
     

        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            PartVisualHandler part = hit.collider.GetComponent<PartVisualHandler>();
            if (part != null)
            {
                // Handle part click
                selectedPart = part.partType;
                ShowPartOptions();
            }

        }

    }

    public void SetUpAvailableUnits()
    {
        /*
        string[] savedUnits = MechaBuilder.ListSavedMechas();
        if(savedUnits.Length == 0)
        {
            Debug.Log("No saved units found. Creating them");
            MechaBuilder.CreateMechasFromUnitData(registry.allUnits.ToArray(),registry);
            savedUnits = MechaBuilder.ListSavedMechas();
            
        }
        */

        
        string[] savedUnits = MechaBuilder.ListSavedMechas();

        if(savedUnits.Length == 0 || savedUnits == null)
        {
            Debug.Log("No saved units found. Creating them");
            MechaBuilder.CreateMechasFromUnitData(registry.allUnits.ToArray(), registry);
            savedUnits = MechaBuilder.ListSavedMechas();
        }

        List<string> unitsIdsList = new List<string>();
        foreach (string unitId in savedUnits)
        {
            unitsIdsList.Add(unitId.ToLower());
        }
        /*
        foreach (UnitData unitInRegistry in registry.allUnits)
        {
            if (unitsIdsList.Contains(unitInRegistry.id)) continue;
            MechaBuilder.SaveMechaFromUnitData(unitInRegistry, registry);
        }
        */
        Unit unit = MechaBuilder.CreateUnitFromRuntimeData(MechaBuilder.LoadFromJson(savedUnits[0]), registry, UnitManager.gameObject);
        UnitManager.SetUnit(unit);
        SetUpModuleSlots();


        /*
        foreach (string unitName in savedUnits)
        {
            GameObject unitButton = Instantiate(availableUnitsPanel, availableUnitsPanel.transform.position, Quaternion.identity, availableUnitsPanel.transform);
            unitButton.GetComponent<UnitButtonUI>().SetUp(unitName, this);
        }
        */
    }

    
    private void RefreshMechaList()
    {
        mechaDropdown.ClearOptions();
        var saved = MechaBuilder.ListSavedMechas();
        mechaDropdown.AddOptions(new System.Collections.Generic.List<string>(saved));
    }

    public void OnMechaSelected(int index)
    {
        DeleteUI();
        string[] savedUnits = MechaBuilder.ListSavedMechas();
        string selectedMecha = savedUnits[index];
        UnitRuntimeData data = MechaBuilder.LoadFromJson(selectedMecha);
        Unit unit = MechaBuilder.CreateUnitFromRuntimeData(data, registry, UnitManager.gameObject);
        UnitManager.SetUnit(unit);
        currentIndex = index;
        SetUpModuleSlots();
    }

    public void ShowPartOptions()
    {
        // Hide all part options first
        for (int i =0; i< availablePartsPanel.transform.childCount;i++)
        {
            Destroy(availablePartsPanel.transform.GetChild(i).gameObject);
        }
        // Show the relevant part options based on the selected part type
        List<UnitPartData> parts = registry.allParts;
        foreach (UnitPartData part in parts)
        {
            if ((int)part.partType == (int)selectedPart)
            {
                GameObject unitButton = Instantiate(buttonInstance,availablePartsPanel.transform);
                unitButton.GetComponent<UnitButtonUI>().SetUp(part,this);
                availablePartsUI.Add(unitButton);
            }
        }
    }

    public void ShowAvailableModules(ModuleData currentAssignModule, ModulePositionType posT, ModuleWeightType weightT, ModuleSlot slot)
    {

        foreach(Transform child in availableModulesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        List<ModuleData> modules = registry.allModules;

        //Empty button slot
        GameObject emptyButton = Instantiate(moduleButtonInstance, availableModulesPanel.transform);
        emptyButton.GetComponent<ModuleButtonUI>().SetUp(null, this, slot);

        foreach (ModuleData module in modules)
        {
            if (module.positionType == posT && module.weightType == weightT && module != currentAssignModule)
            {
                GameObject unitButton = Instantiate(moduleButtonInstance, availableModulesPanel.transform);
                unitButton.GetComponent<ModuleButtonUI>().SetUp(module, this, slot);
                availableModulesUI.Add(unitButton);
            }
        }
    }

    public void SetUpModuleSlots()
    {
        List<Transform> slots = new List<Transform>();
        foreach(var part in UnitManager.unit.Parts) { 
            foreach (var slot in part.slots)
            {
                GameObject moduleSlotButton = Instantiate(moduleSlotInstance, GameObject.FindGameObjectWithTag("Canvas").transform);
                moduleSlotButton.GetComponent<ModuleSlotUI>().SetUp(slot);

                moduleSlots.Add(moduleSlotButton);
            }
        }
        
    }

    public void HideSlotbackgrounds()
    {
        foreach (GameObject slot in moduleSlots)
        {
            slot.GetComponent<ModuleSlotUI>().background.SetActive(false);
        }
    }

    public void ChangeMechaPart(UnitPartData partData)
    {
        DeleteUI();
        UnitManager.transform.GetComponent<UnitVisualManager>().ChangePart(MechaBuilder.ListSavedMechas()[currentIndex], partData,registry);
        SetUpModuleSlots();
    }

    public void ChangeModulePart(ModuleData moduleData, ModuleSlot slot)
    {
        DeleteUI();
        UnitManager.transform.GetComponent<UnitVisualManager>().AddModule(MechaBuilder.ListSavedMechas()[currentIndex], moduleData, slot, registry);
        SetUpModuleSlots();
    }

    public void DeleteUI()
    {
        foreach (GameObject part in availablePartsUI)
        {
            Destroy(part);
        }
        availablePartsUI.Clear();
        foreach (GameObject module in availableModulesUI)
        {
            Destroy(module);
        }
        availableModulesUI.Clear();
        foreach (GameObject slot in moduleSlots)
        {
            Destroy(slot);
        }
        moduleSlots.Clear();
    }

    public void ExitHangar()
    {
        GameManager.instance.ChangeScene(SceneEnum.HubScene);
    }

}
