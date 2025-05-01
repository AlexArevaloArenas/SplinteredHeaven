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
        SetUp();
    }


    public void StartGame()
    {
        //gameManager.StartGame();
        fadeControl.UseFade();
    }
    public void ExitGame()
    {
        //gameManager.currentState = State.Menu;
        fadeControl.UseFade();
        //ShowCurrentMenu();
    }
    public void ContinueGame()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.acceptSFX,Camera.main.transform.position);
        EventManager.Instance.StartContinueGameEvent();
        fadeControl.UseFade();
    }

    public void PauseGame()
    {
        //gameManager.currentState = State.Pause;
        fadeControl.UseFade();
    }
    public void OptionsGame()
    {
        //gameManager.currentState = State.Options;
        fadeControl.UseFade();
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
        /*
        pauseMenu.SetActive(false);
        gameHUD.SetActive(false);
        optionsMenu.SetActive(false);
        */
    }

    public void HideAllMenus()
    {
        mainMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        //gameHUD.SetActive(false);
        //optionsMenu.SetActive(false);
    }
}
