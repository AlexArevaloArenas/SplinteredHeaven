using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public GameObject missionPanel;
    public GameObject missionHolder;
    public GameObject missionPrefab;
    public GameObject map;
    public RectTransform maskRectTransform;

    public GameObject[] mapLocations;
    public List<MissionButton> missionButtons;

    private bool isOpen = false;

    private void Start()
    {
        missionPanel.SetActive(false);
    }

    public void OpenMenu()
    {
        if (isOpen) return;
        isOpen = true;
        EventManager.Instance.FixPlayerMovement();
        missionPanel.SetActive(true);
        
        foreach (MissionData mission in MissionManager.Instance.availableMissions)
        {
            GameObject missionObject = Instantiate(missionPrefab, missionHolder.transform);
            missionButtons.Add(missionObject.GetComponent<MissionButton>());
            missionObject.GetComponent<MissionButton>().missionUI = this;
            missionObject.GetComponent<MissionButton>().missionData = mission;
            missionObject.GetComponent<MissionButton>().SetTittle(mission.title);
            missionObject.GetComponent<MissionButton>().SetDescription(mission.description);


            foreach (GameObject location in mapLocations)
            {
                if (location.name == mission.location.ToString())
                {
                    missionObject.GetComponent<MissionButton>().mapLocation = location;
                }
            }

            if (mission == MissionManager.Instance.availableMissions[0])
            {
                missionObject.GetComponent<MissionButton>().Select();
                
            }
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        if (!isOpen) return;
        isOpen = false;
        EventManager.Instance.FreePlayerMovement();
        
        foreach (Transform child in missionHolder.transform)
        {
            Destroy(child.gameObject);
        }

        missionPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void SetMapPlace(GameObject place)
    {
        RectTransform placeRect = place.GetComponent<RectTransform>();

        // Convert place's world position to local position relative to the mask
        Vector2 localPointInMask;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(maskRectTransform,RectTransformUtility.WorldToScreenPoint(null, placeRect.position),null,out localPointInMask);

        // Move the map so that the place aligns with the center of the mask
        //map.GetComponent<RectTransform>().localPosition -= (Vector3)localPointInMask;
        Vector3 newPosition = map.GetComponent<RectTransform>().localPosition - (Vector3)localPointInMask;

        StartCoroutine(MoveMapToPosition(newPosition, 0.5f));
    }

    public IEnumerator MoveMapToPosition(Vector3 position, float duration)
    {
        Vector3 startPosition = map.GetComponent<RectTransform>().localPosition;
        Vector3 endPosition = position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            map.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        map.GetComponent<RectTransform>().localPosition = endPosition;
    }

    public void StartMission()
    {
        foreach (MissionButton button in missionButtons)
        {
            if (button.isSelected)
            {
                MissionManager.Instance.StartMission(button.missionData);
                CloseMenu();
                return;
            }
        }
    }

}
