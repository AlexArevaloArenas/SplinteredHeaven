using UnityEditor;
using UnityEngine;

public class StartBattle : MonoBehaviour, IInteractable
{

    [SerializeField] string interactionText;
    [SerializeField] SceneAsset newScene;

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact(GameObject interactor)
    {
        EventManager.Instance.StartChangeSceneEvent(newScene.name);
    }

}
