using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    //[SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup backgroundGroup;
    [SerializeField] private MenuManager menu;

    private void Start()
    {
        backgroundGroup = GetComponent<CanvasGroup>();
    }

    public void UseFade()
    {
        Debug.Log("Start Fade!");
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        while (backgroundGroup.alpha < 1)
        {
            //Debug.Log("Start Fading In");
            backgroundGroup = GetComponent<CanvasGroup>();
            backgroundGroup.alpha += 0.05f;
            if (backgroundGroup.alpha >= 1)
            {
                //Debug.Log("Start Fading Out");
                //menu.ShowCurrentMenu();
                menu.HideAllMenus();
                Destroy(Camera.main);
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
            backgroundGroup.alpha -= 0.05f;
            if (backgroundGroup.alpha <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(.02f);
        }
    }
}
