using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionDisplay : MonoBehaviour
{
    private List<GameObject> selectionSpaces;
    public UnitSelections selections;

    private void OnEnable()
    {
        //Get unit spaces
        selectionSpaces = new List<GameObject>();
        int selectionSpaceNumber = this.transform.GetChild(0).GetChild(0).childCount;
        /*
        for (int i = 0; i < selectionSpaceNumber; i++)
        {
            selectionSpaces.Add(transform.GetChild(0).GetChild(i).gameObject);
        }
        */
        //Set event

    }

    private void OnDisable()
    {
        
    }

    private void SetSelections()
    {

    }

}
