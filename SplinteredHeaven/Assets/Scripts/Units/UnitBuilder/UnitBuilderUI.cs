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
    public GameObject uiPanel;

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

    public TMPro.TextMeshProUGUI unitInfo;

    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraHangar;
    private void Start()
    {
        EventManager.Instance.OpenBuildMecha += OpenEdit;
        EventManager.Instance.LeftMouseDownEvent += Click;
        EventManager.Instance.EscapeKeyEvent += ExitEdit;
        EventManager.Instance.StartBlockInteraction(true);
        gameObject.SetActive(false); // Disable the UnitBuilderUI at start
        
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

    private void OnEnable()
    {
        EventManager.Instance.OpenBuildMecha += OpenEdit;
        EventManager.Instance.LeftMouseDownEvent += Click;
        EventManager.Instance.EscapeKeyEvent += ExitEdit;
        EventManager.Instance.StartBlockInteraction(true);

        SetUpAvailableUnits();
        RefreshMechaList();
    }

    private void OnDisable()
    {
        EventManager.Instance.LeftMouseDownEvent -= Click;
        EventManager.Instance.OpenBuildMecha -= OpenEdit;
        EventManager.Instance.EscapeKeyEvent -= ExitEdit;
        EventManager.Instance.StartBlockInteraction(false);
        EventManager.Instance.FreePlayerMovement();
    }

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

        if (Input.GetMouseButton(0))
        {
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
        RefreshInfoText(unit);

        UnitManager.transform.rotation = Quaternion.Euler(0, -90, 0);
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
        RefreshInfoText(unit);

        UnitManager.transform.rotation = Quaternion.Euler(0, 90, 0);
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
//------------------------------------------------------------------------------------------------------------------------------------------
            //FIND A BETTER FIX IN THE FUTURE
            if (part.id.Contains("Tank") || part.id.Contains("Kaiju"))
            {
                continue;
            }
//------------------------------------------------------------------------------------------------------------------------------------------

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
        foreach (Transform child in availableModulesPanel.transform)
        {
            Destroy(child.gameObject);
        }
        List<ModuleData> modules = registry.allModules;

        //Empty button slot
        GameObject emptyButton = Instantiate(moduleButtonInstance, availableModulesPanel.transform);
        emptyButton.GetComponent<ModuleButtonUI>().SetUp(null, this, slot);

        foreach (ModuleData module in modules)
        {
            //--------------------------------------------------------------------------------------------------
            //FIND A BETTER FIX IN THE FUTURE
            if (module.id.Contains("Tank") || module.id.Contains("Kaiju"))
            {
                continue;
            }
            //--------------------------------------------------------------------------------------------------
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
                moduleSlotButton.GetComponent<ModuleSlotUI>().moduleSlotText.text = part.name;
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

    public void RefreshInfoText(Unit unit)
    {
        unitInfo.text = $"<b>Unit</b>: {unit.name}\n\n" +
                        $"<b>Health</b>: {unit.currentHealth}/{unit.maxHealth}\n\n" +
                        $"<b>Speed</b>: {unit.speed}\n\n" +
                        $"<b>Vision Range</b>: {unit.visionRange}\n\n" +
                        $"<b>Detection Range</b>: {unit.detectionRange}";
    }

    public IEnumerator DoAfterWait(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }

    public void OpenEdit()
    {
        TimeManager.Instance.Stop();
        UnityEngine.Cursor.lockState = CursorLockMode.Confined; // Unlock the cursor
        UnityEngine.Cursor.visible = true; // Make the cursor visible
        uiPanel.SetActive(true); // Enable the UI panel
        Fader.Instance.FadeAndDo(() =>
        {
            player.SetActive(false); // Disable player
            cameraHangar.SetActive(true); // Disable player
        }, 0.005f);
    }

    public void ExitEdit()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
        UnityEngine.Cursor.visible = false; // Make the cursor visible

        TimeManager.Instance.Play();
        uiPanel.SetActive(false); // Disable the UI panel
        DeleteUI(); // Clear the UI elements
        EventManager.Instance.StartBlockInteraction(false); // Close any open build mecha UI
        gameObject.SetActive(false); // Disable the UnitBuilderUI
        Fader.Instance.FadeAndDo(() =>
        {
            cameraHangar.SetActive(false); // Disable player
            player.SetActive(true); // Disable player
            gameObject.SetActive(false); // Disable the UnitBuilderUI
        }, 0.01f);
    }

}
