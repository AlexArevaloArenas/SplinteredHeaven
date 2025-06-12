using DG.Tweening;
using Unity.AppUI.UI;
using UnityEngine;
using static RadialMenuEntry;

public class DialogueSelectorAnimation : MonoBehaviour
{
    RectTransform rect;
    private bool wait = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(wait || gameObject.activeSelf == false) return;
        wait = true;
        rect.DOShakePosition(1f, 8f, 1, 20, false, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            wait = false;
        });

    }
}
