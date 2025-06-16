using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Fader fadeControl;

    public GameManager gameManager;

    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject buttonPanel;
    /*
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameHUD;
    
    */

    public Slider sliderMaster;
    public Slider sliderMusic;
    public Slider sliderSFX;
    public Slider sliderAmbience;
    

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        fadeControl = Fader.GetInstance();
        CloseOptions();
    }


    public void ContinueGame()
    {
        fadeControl.StartFade("ContinueGameEvent");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.acceptSFX, Camera.main.transform.position);

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void HideMainButtons()
    {
               buttonPanel.SetActive(false);
        // Fade out the button panel
        StartCoroutine(fadeOutObj(buttonPanel));
    }
    public void ShowMainButtons()
    {
        buttonPanel.SetActive(true);
        // Fade in the button panel
        StartCoroutine(fadeInObj(buttonPanel));
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        // Fade in the options panel
        StartCoroutine(fadeInObj(optionsPanel));
        HideMainButtons();
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        // Fade out the options panel
        StartCoroutine(fadeOutObj(optionsPanel));
        ShowMainButtons();
    }

    public void ShowCredits()
    {
        StartCoroutine(showCredits());
    }

    public IEnumerator showCredits()
    {
        while (creditsPanel.GetComponent<CanvasGroup>().alpha < 1)
        {
            creditsPanel.GetComponent<CanvasGroup>().alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(10f);
        while (creditsPanel.GetComponent<CanvasGroup>().alpha > 0)
        {
            creditsPanel.GetComponent<CanvasGroup>().alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
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

    //OPTIONS MENU
    public void SetMasterVolume()
    {
        AudioManager.instance.SetVolume(sliderMaster.value, VolumeType.Master);
    }
    public void SetMusicVolume()
    {
        AudioManager.instance.SetVolume(sliderMusic.value, VolumeType.Music);
    }
    public void SetSFXVolume()
    {
        AudioManager.instance.SetVolume(sliderSFX.value, VolumeType.SFX);
    }
    public void SetAmbienceVolume()
    {
        AudioManager.instance.SetVolume(sliderAmbience.value, VolumeType.Ambience);
    }



    /*
    public void StartGame()
    {
        //gameManager.StartGame();
        fadeControl.StartFade();
    }
    public void ExitGame()
    {
        //gameManager.currentState = State.Menu;
        fadeControl.StartFade();
        //ShowCurrentMenu();
    }
    public void PauseGame()
    {
        //gameManager.currentState = State.Pause;
        fadeControl.StartFade();
    }
    
    public void OptionsGame()
    {
        //gameManager.currentState = State.Options;
        fadeControl.StartFade();
    }
    
    public void ShowCurrentMenu()
    {
        HideAllMenus();
        switch (gameManager.currentState)
        {
            case State.Menu:
                mainMenu.SetActive(true);
                break;
            case State.FPHub:
                gameHUD.SetActive(true);
                break;
            case State.Options:
                optionsMenu.SetActive(true);
                break;
            case State.Pause:
                pauseMenu.SetActive(true);
                break;
            default:
                break;
        }
    }
    

    public void SetUp()
    {
        mainMenu.SetActive(true);
        
        
    }

    public void HideAllMenus()
    {
        mainMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        //gameHUD.SetActive(false);
        //optionsMenu.SetActive(false);
    }
*/
}

