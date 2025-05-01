using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class RadialMenu : MonoBehaviour
{
    public bool toggle = false;
    [SerializeField]
    GameObject entryPrefab;

    [SerializeField]
    List<Texture> Icons;
    List<RadialMenuEntry> entries;

    [SerializeField]
    RawImage TargetIcon;

    [SerializeField]
    float Radius = 12.0f;

    RectTransform rectTransform;

    void Start()
    {
        entries = new List<RadialMenuEntry>();
        //EventManager.Instance.OpenRadialMenu += Open;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        
    }

    public void AddEntry(string label, Texture pIcon, RadialMenuEntry.RadialMenuEntryDelegate pCallback)
    {
        GameObject entry = Instantiate(entryPrefab, transform);

        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        rme.SetLabel(label);
        rme.SetIcon(pIcon);
        rme.SetCallback(pCallback);

        entries.Add(rme);
    }

    public void Open(int entryNum)
    {
        for (int i = 0; i < entryNum; i++)
        {
            AddEntry("Button"+i.ToString(), Icons[i], SetTargetIcon);
        }
        Rearreange();
    }
    
    public void Close()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject entry = entries[i].gameObject;
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            
            rect.DOAnchorPos(Vector3.zero, .3f).SetEase(Ease.OutQuad).SetDelay(0.05f * i).onComplete = delegate()
            {
                Destroy(entry);
            };
        }
        entries.Clear();
    }

    public void Toggle(string type)
    {
        if (entries.Count == 0)
        {
            toggle = true;
            switch (type)
            {
                case "Module":
                    //Open(UnitSelections.Instance.unitSelected[0]);
                    break;
                default:
                    break;
            }
            

        }
        else
        {
            toggle = false;
            Close();
        }
    }

    void Rearreange()
    {
        float radiansOfSeparation = (2 * Mathf.PI) / entries.Count;
        for (int i = 0; i < entries.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeparation * i) * Radius;
            float y = Mathf.Cos(radiansOfSeparation * i) * Radius;

            //entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            rect.localScale =Vector3.zero;
            rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(0.05f*i);
            rect.DOAnchorPos(new Vector3(x, y,0), .3f).SetEase(Ease.OutQuad).SetDelay(0.05f*i);
        }
    }

    void SetTargetIcon(RadialMenuEntry entry)
    {
        //TargetIcon.texture = entry.GetIcon();
    }
}
