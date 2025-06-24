using System.Collections;
using TMPro;
using UnityEngine;

public class FP_UIManager : MonoBehaviour
{
    private bool interactionAvailable;
    [SerializeField] private GameObject interactionInformer;
    private string interactionName;

    public GameObject menuPanel;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //EventManager.Instance.FPDialogueEvent += StartDialogue;
        EventManager.Instance.FPChangeInteractionSymbolEvent += ShowSymbolStatus;
        EventManager.Instance.EscapeKeyEvent += () => OpenInGameMenu();

    }

    private void OnDisable()
    {
        EventManager.Instance.FPChangeInteractionSymbolEvent -= ShowSymbolStatus;
        EventManager.Instance.EscapeKeyEvent -= () => OpenInGameMenu();
    }

    private void Update()
    {
        if (interactionAvailable)
        {
            interactionInformer.GetComponent<TextMeshProUGUI>().text = interactionName;
        }
        interactionInformer.SetActive(interactionAvailable);
    }

    private void ShowSymbolStatus(bool show, string interactionText)
    {
        interactionAvailable = show;
        interactionName = interactionText;
    }

    public void OpenInGameMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        EventManager.Instance.FixPlayerMovement();

        menuPanel.SetActive(true);
        StartCoroutine(fadeInObj(menuPanel));
    }

    public void CloseInGameMenu()
    {
        EventManager.Instance.FreePlayerMovement();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(fadeOutObj(menuPanel));
        menuPanel.SetActive(false);
    }

    public void BackToStartScene()
    {
        Fader.Instance.StartFade("BackToMainMenuEvent");
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

}
