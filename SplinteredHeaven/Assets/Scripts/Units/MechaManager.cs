using UnityEngine;

public class MechaManager : CharacterManager
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        if (unitData is not MechaData)
        {
            Debug.LogError("MechaManager is not assigned a MechaData object.");
        }
        else
        {
            // Initialize the Mecha with its data
            InitializeMecha();
        }
        // Call the base class Start method
        base.Start();
    }


    private void InitializeMecha()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
