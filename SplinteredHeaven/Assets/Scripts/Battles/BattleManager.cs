using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

public class BattleManager : MonoBehaviour
{
    public GameObject menuPanel; // Reference to the battle menu panel
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    List<GameObject> unitsSelected;
    //Module abilities
    public GameObject moduleAbilitiesPanel;
    public GameObject moduleAbilitiesButtonPrefab;

    private List<GameObject> moduleButtons = new List<GameObject>();

    //
    public TMPro.TextMeshProUGUI textName;
    public TMPro.TextMeshProUGUI textDescription;
    public TMPro.TextMeshProUGUI objectivesText;

    public Transform[] playerSpawnPoints;
    public GameObject unitPrefab; // Reference to the general unit manager for the player

    public GameObject victoryPanel;
    public GameObject defeatPanel;

    void Start()
    {
        EventManager.Instance.EscapeKeyEvent += () => OpenBattleMenu();
        EventManager.Instance.MissionStartsEvent += LoadMission;

        MissionEvents.OnObjectiveCompleted += OnObjectiveCompleted;
        MissionEvents.OnObjectiveFailed += OnObjectiveFailed;

        //MissionEvents.OnObjectiveCompleted +=
        //MissionEvents.OnObjectiveFailed += () => EndBattle(false);

        menuPanel.SetActive(false); // Ensure the menu panel is initially hidden
        menuPanel.GetComponent<CanvasGroup>().alpha = 0; // Set initial alpha to 0 for fade-in effect
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayerUnits()
    {

    }

    public void StartEnemyAI()
    {

    }

    public void EndBattle(bool playerWon)
    {
        if (playerWon)
        {
            Debug.Log("Player won the battle!");
        }
        else
        {
            Debug.Log("Player lost the battle.");
        }
    }

    public void ResetBattle()
    {
        Debug.Log("Battle has been reset.");
        // Additional logic to reset the battle state can be added here
    }

    public void OpenBattleMenu()
    {
        if(menuPanel.activeSelf)
        {
            CloseBattleMenu();
            return;
        }
        EventManager.Instance.FixPlayerMovement();
        menuPanel.SetActive(true);
        StartCoroutine(fadeInObj(menuPanel));
    }
    public void CloseBattleMenu()
    {
        EventManager.Instance.FreePlayerMovement();
        StartCoroutine(fadeOutObj(menuPanel));
        menuPanel.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public IEnumerator fadeInObj(GameObject obj)
    {
        while (obj.GetComponent<CanvasGroup>().alpha < 1)
        {
            obj.GetComponent<CanvasGroup>().alpha += 0.02f;
            yield return new WaitForSeconds(0.001f);
        }

    }
    public IEnumerator fadeOutObj(GameObject obj)
    {
        while (obj.GetComponent<CanvasGroup>().alpha > 0)
        {
            obj.GetComponent<CanvasGroup>().alpha -= 0.02f;
            yield return new WaitForSeconds(0.001f);
        }
    }


    public void ReloadUI()
    {
        ClearUI();
        unitsSelected = UnitSelections.Instance.unitSelected;
        if (unitsSelected == null || unitsSelected.Count == 0)
        {
            Debug.LogWarning("No units selected to reload UI.");
            return;
        }
        UnitManager unitSelected = unitsSelected[0].GetComponent<UnitManager>();

        foreach (ModuleInstance module in unitSelected.unit.Parts.SelectMany(p => p.Modules))
        {
            if (module is WeaponModuleInstance)
            {
                continue;
            }
            GameObject button = Instantiate(moduleAbilitiesButtonPrefab, moduleAbilitiesPanel.transform);
            button.GetComponent<ModuleAbilityButton>().Init(module, unitSelected, module.Data.icon, module.Data.id);
            moduleButtons.Add(button);
        }

        textName.text = unitSelected.unit.unitData.name;
        textDescription.text = unitSelected.unit.unitData.description;

    }
    public void ClearUI()
    {
        foreach (GameObject button in moduleButtons)
        {
            Destroy(button);
        }
        moduleButtons.Clear();
        textName.text = string.Empty;
        textDescription.text = string.Empty;
    }

    private void LoadMission(MissionInstance mission)
    {
        objectivesText.text = "Objectives:\n";
        //Load UI
        foreach (ObjectiveInstance objData in mission.objectives)
        {
            objectivesText.text += $"- {objData.definition.description}\n";
        }

        //Load Units
        foreach (Unit unit in GameManager.instance.playerCurrentArmy)
        {
            Transform point = playerSpawnPoints.Random();
            GameObject unitObj = Instantiate(unitPrefab, point.position, point.rotation);
            unitObj.GetComponent<UnitManager>().SetUnit(unit);
        }

    }

    public void OnObjectiveCompleted(ObjectiveInstance instance)
    {
        Debug.Log($"Objective Completed: {instance.definition.description}");
        //objectivesText.text += $"Objective Completed: {instance.definition.description}\n";
        objectivesText.text = objectivesText.text.Replace($"- {instance.definition.description}\n", $"{instance.definition.description} ?\n");
    }

    public void OnObjectiveFailed(ObjectiveInstance instance)
    {
        objectivesText.text += $"Objective Failed: {instance.definition.description}\n";
        Invoke(nameof(EndBattle), 2f); // Delay to show failure message
    }

}
