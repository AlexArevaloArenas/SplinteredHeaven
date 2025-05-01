using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCheck : MonoBehaviour
{
    [SerializeField] public SceneAsset controlScene;
    private void Awake()
    {
        //bool controlSceneLoaded = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == controlScene.name)
            {
                //controlSceneLoaded=true;
                return;
            }
        }
        SceneManager.LoadScene(controlScene.name, LoadSceneMode.Additive);

    }

}
