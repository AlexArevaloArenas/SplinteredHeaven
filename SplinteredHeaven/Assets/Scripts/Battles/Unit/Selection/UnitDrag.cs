//using System.Collections;
//using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    Camera myCam;

    //Graphical
    [SerializeField]
    RectTransform boxVisual;

    //logical
    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    //Input
    bool held=true;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.LeftMouseHeldEvent += StartMouseHeld;
        EventManager.Instance.LeftMouseDownEvent += MousePressed;
        EventManager.Instance.LeftMouseUpEvent += MouseReleased;


        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void OnDestroy()
    {
        EventManager.Instance.LeftMouseHeldEvent -= StartMouseHeld;
        EventManager.Instance.LeftMouseDownEvent -= MousePressed;
        EventManager.Instance.LeftMouseUpEvent -= MouseReleased;
    }

    private void Update()
    {
        if (held)
        {
            MouseHeld();
        }
    }

    private void MousePressed()
    {
        startPosition = Input.mousePosition;
        selectionBox = new Rect();
    }

    private void StartMouseHeld()
    {
        held = true;
    }

    private void MouseHeld()
    {
        endPosition = Input.mousePosition;
        DrawVisual();
        DrawSelection();
    }

    private void MouseReleased()
    {
        held = false;
        SelectUnits();
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd)/2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x-boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            //draggin left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            //dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        //y calc
        if (Input.mousePosition.y < startPosition.y)
        {
            //dragging down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }
    void SelectUnits()
    {
        //loop all units
        foreach (var unit in UnitSelections.Instance.unitList)
        {
            //check if they are inside the box
            if(selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelections.Instance.DragSelect(unit);
                Debug.Log("Drag Works");
            }
        }

        //Overlay.Instance.ShowUnitSelections();
    }

}
