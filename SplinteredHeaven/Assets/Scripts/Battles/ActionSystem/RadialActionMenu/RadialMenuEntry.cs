using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RadialMenuEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public delegate void RadialMenuEntryDelegate(RadialMenuEntry entry);

    [SerializeField]
    TextMeshProUGUI Label;

    [SerializeField]
    RawImage Icon;

    RectTransform rect;

    RadialMenuEntryDelegate Callback;

    private void Start()
    {
        rect = Icon.GetComponent<RectTransform>();
        //rect.localScale = Vector3.zero;
    }

    public void SetLabel(string label)
    {
        Label.text = label;
    }

    public void SetIcon(Texture picon)
    {
        Icon.texture = picon;
    }
    public Texture GetIcon()
    {
        return Icon.texture;
    }

    public void SetCallback(RadialMenuEntryDelegate callback)
    {
        Callback = callback;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOComplete();
        rect.DOScale(Vector3.one*1.5f,.3f).SetEase(Ease.OutQuad);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOComplete();
        rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Callback?.Invoke(this);
    }
}
