using UnityEditor;
using UnityEngine;

public class OpenMissionSelector : MonoBehaviour, IInteractable
{
    public MissionUI missionUI;
    [SerializeField] string interactionText;
    

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(GameObject interactor)
    {
        missionUI.OpenMenu();
    }

}
