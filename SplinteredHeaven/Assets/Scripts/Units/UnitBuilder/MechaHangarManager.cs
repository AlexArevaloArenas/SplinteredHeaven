using System.Collections;
using UnityEditor;
using UnityEngine;

public class MechaHangarManager : MonoBehaviour
{
    public static MechaHangarManager instance;
    public UnitManager unitExample;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (unitExample.mechaClass == MechaClass.empty && unitExample.unitData != null)
        {
            Unit unit = new Unit(unitExample.unitData, unitExample.gameObject);
            unitExample.unit = unit;
            SaveMecha(unitExample.unit);
        }
    }

    public void SaveMecha(Unit unit)
    {
        //string unitJsonData = JsonUtility.ToJson(unit);
        //string path = "Assets/Resources/UnitJSON/" + unit.name + ".json";
        //System.IO.File.WriteAllText(path, unitJsonData);
        string json = JsonUtility.ToJson(unitExample.unit);
        TextAsset text = new TextAsset(json);
        AssetDatabase.CreateAsset(text, $"Assets/Resources/UnitJSON/{unit.name}.json");
        unitExample.GetComponent<UnitVisualManager>().Init(unit);
        //unitExample.unitJsonData = text;
        //Debug.Log(unitExample.unitJsonData.text);
    }

    public void BuildMecha(Unit unit)
    {

        // Clear existing parts
        GetComponent<UnitVisualManager>().ClearParts();
        // Load new parts from unit data
        GetComponent<UnitVisualManager>().LoadPartsFromData(unit);

    }

}
