using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Fader fadeControl;

    public GameManager gameManager;

    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameHUD;
    public GameObject optionsMenu;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fadeControl = Fader.GetInstance();
    }


    public void ContinueGame()
    {
        fadeControl.StartFade("ContinueGameEvent");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.acceptSFX, Camera.main.transform.position);

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

