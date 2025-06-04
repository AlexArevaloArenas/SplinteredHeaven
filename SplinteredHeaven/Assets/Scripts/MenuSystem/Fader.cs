using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    //[SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup backgroundGroup;
    [SerializeField] private MenuManager menu;
    public float fadeSpeed = 0.02f;


    public static Fader Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        //dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        //inkExternalFunctions = new InkExternalFunctions();

        //audioSource = this.gameObject.AddComponent<AudioSource>();
        //currentAudioInfo = defaultAudioInfo;
    }

    private void Start()
    {
        //backgroundGroup = currentUI.GetBackgroundGroup();
    }

    public static Fader GetInstance()
    {
        return Instance as Fader;
    }

    public void StartFade(String eventString)
    {
        Debug.Log("Start Fade!");
        StartCoroutine(FadeIn(eventString));
    }

    IEnumerator FadeIn(String nextAction)
    {
        while (backgroundGroup.alpha < 1)
        {
            //Debug.Log("Start Fading In");
            backgroundGroup = GetComponent<CanvasGroup>();
            backgroundGroup.alpha += fadeSpeed;
            if (backgroundGroup.alpha >= 1)
            {
                //Debug.Log("Start Fading Out");
                //menu.ShowCurrentMenu();
                //menu.HideAllMenus();
                //Destroy(Camera.main);
                EventManager.Instance.StartEventByString(nextAction);
                StartCoroutine("FadeOut");
                break;
            }
            yield return new WaitForSeconds(.02f);
        }

    }

    IEnumerator FadeOut()
    {
        while (backgroundGroup.alpha > 0)
        {
            backgroundGroup = GetComponent<CanvasGroup>();
            backgroundGroup.alpha -= fadeSpeed;
            if (backgroundGroup.alpha <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(.02f);
        }
    }
}
