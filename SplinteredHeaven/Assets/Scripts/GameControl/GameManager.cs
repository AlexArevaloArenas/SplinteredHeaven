using Ink.Parsed;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public State currentState;
    public string playerFaction;

    //Singleton pattern
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        //DontDestroyOnLoad(gameObject);

        //Load game data
        DataHandler.LoadGameData();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //EVENT SUB
        EventManager.Instance.ContinueGameEvent += ContinueGame;

        EventManager.Instance.ChangeSceneEvent += ChangeScene;
    }

    private void ContinueGame()
    {
        SceneManager.LoadScene("HubScene",LoadSceneMode.Additive);
    }

    public void ChangeScene(SceneEnum scene)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum State
{
    Menu,
    FPHub,
    EditorHub,
    ResearchHub,
    Battle,
    Pause,
    Options,
}

public enum SceneEnum
{
    MainMenu,
    HubScene,
    EditorHub,
    ResearchHub,
    Battle,
    Pause,
    Options,
}