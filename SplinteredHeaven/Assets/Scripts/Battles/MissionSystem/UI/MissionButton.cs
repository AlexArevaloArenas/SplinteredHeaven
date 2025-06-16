using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MissionButton : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    public GameObject selector;
    public bool isSelected = false;
    public MissionData missionData;

    public TMPro.TMP_Text titleText;
    public TMPro.TMP_Text descriptionText;

    public GameObject mapLocation;

    public MissionUI missionUI;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rectTransform=GetComponent<RectTransform>();
    }

    public void Select()
    {
        foreach (MissionButton button in missionUI.missionButtons)
        {
            if (button.isSelected)
            {
                button.Deselect();
            }
        }
        selector.SetActive(true);
        isSelected = true;
        rectTransform.DOScale(1.05f, 0.2f).SetEase(Ease.OutBack);
        if (missionUI != null)
        {
            missionUI.SetMapPlace(mapLocation);
        }
    }

    public void Deselect()
    {
        selector.SetActive(false);
        isSelected = false;
        rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void SetTittle(string text)
    {
        titleText.text = text;
    }

    public void SetDescription(string text)
    {
        descriptionText.text = text;
    }

    private void OnEnable()
    {
        Deselect();
    }

    private void OnDestroy()
    {
        Deselect();
    }

    private void OnDisable()
    {
        Deselect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
        MissionManager.Instance.selectedMissionData = missionData;
    }
}
