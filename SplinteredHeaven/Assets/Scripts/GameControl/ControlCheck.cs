
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCheck : MonoBehaviour
{
    //[SerializeField] public Scene controlScene;
    private void Awake()
    {
        //bool controlSceneLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "ControlScene")
            {
                //controlSceneLoaded=true;
                return;
            }
        }
        SceneManager.LoadScene("ControlScene", LoadSceneMode.Additive);

    }

}
