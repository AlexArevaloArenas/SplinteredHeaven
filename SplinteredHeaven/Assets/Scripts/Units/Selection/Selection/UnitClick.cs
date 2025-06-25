//using System.Collections;
//using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;
    
    public LayerMask selectable;
    public LayerMask terrain;
    public LayerMask UI;
    public GameObject terrainMark;
    public LayerMask allLayers; // This should include all layers you want to check against, including terrain and selectable layers
    public BattleManager battleManager;

    void Start()
    {
        myCam = Camera.main;

        EventManager.Instance.LeftMouseDownEvent += SelectClick;
        EventManager.Instance.RightMouseDownEvent += PlayerRightClick;
    }

    public void PlayerRightClick()
    {
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, allLayers, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                foreach (GameObject unit in UnitSelections.Instance.unitSelected)
                {
                    if (unit.tag == "Player")
                    {
                        unit.GetComponent<UnitAI>()?.StartMoveBehaviour(hit.point);
                        StartCoroutine(MoveTerrainMarkCoroutine(0.01f, hit.point));
                    }
                }
            }

            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Selectable"))
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    return;
                }
                foreach (GameObject unit in UnitSelections.Instance.unitSelected)
                {
                    if (unit.tag == "Player")
                    {
                        unit.GetComponent<UnitAI>().Stop();
                        unit.GetComponent<UnitAI>()?.StartAttackBehaviour(hit.collider.gameObject);

                    }
                }
            }
        }

        

    }

    //Create subrutine to move terrain mark after certain delay
    private IEnumerator MoveTerrainMarkCoroutine(float delay, Vector3 point)
    {
        yield return new WaitForSeconds(delay);
        terrainMark.transform.position = new Vector3(point.x, point.y + terrainMark.GetComponent<TerrainMark>()._heightDistance, point.z);
        terrainMark.SetActive(false);
        terrainMark.SetActive(true);  
    }

    public void SelectClick()
    {
        
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectable, QueryTriggerInteraction.Ignore))
        {
            //If we hit a clickable object
            

            if (Input.GetKey(KeyCode.LeftShift))
            {//or shift click
                UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
            }
            else
            {//Or normal click
                UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
            }

            battleManager.ReloadUI();

        }
        else
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            //We don't and we are not shift clicking
            if (!Input.GetKey(KeyCode.LeftShift) && !(results.Where(r => r.gameObject.layer == 5).Count() > 0))
            {
                UnitSelections.Instance.DeselectAll();
                battleManager.ClearUI();
            }
        }
    }

}
