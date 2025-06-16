using Ink.Parsed;
using UnityEditor.SearchService;
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
        EventManager.Instance.BackToMainMenuEvent += () => ChangeScene(SceneEnum.StartScene);
        EventManager.Instance.LoadCurrentMissionSceneEvent += LoadCurrentMissionScene;

        EventManager.Instance.ChangeSceneEvent += ChangeScene;
    }

    private void ContinueGame()
    {
        Debug.Log("Continue Game Event Triggered");
        //SceneManager.LoadScene("HubScene",LoadSceneMode.Additive);
        ChangeScene(SceneEnum.HubScene);
    }

    private void LoadCurrentMissionScene()
    {
        ChangeScene(MissionManager.Instance.currentMission.scene);
    }

    public void ChangeScene(SceneEnum scene)
    {
       //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
       for (int i = 0; i < SceneManager.sceneCount; i++)
       {
            if (SceneManager.GetSceneAt(i).name != "ControlScene")
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
       }
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
    StartScene,
    HubScene,
    EditorHub,
    ResearchHub,
    BattleScene,
    Pause,
    Options,
}