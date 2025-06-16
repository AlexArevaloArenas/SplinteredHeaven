using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject menuPanel; // Reference to the battle menu panel
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    List<GameObject> unitsSelected;
    //Module abilities
    public GameObject moduleAbilitiesPanel;
    public GameObject moduleAbilitiesButtonPrefab;

    private List<GameObject> moduleButtons = new List<GameObject>();

    void Start()
    {
        EventManager.Instance.EscapeKeyEvent += () => OpenBattleMenu();

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

    }
    public void ClearUI()
    {
        foreach (GameObject button in moduleButtons)
        {
            Destroy(button);
        }
        moduleButtons.Clear();

    }

}
