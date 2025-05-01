using UnityEngine;

public class MechaEditor : MonoBehaviour
{

    public GameObject mechaPrefab;

    public GameObject[] mechaPartsHolders;

    //public CoreSO currentCoreData;

    //public PartSO[] mechaList;
    //public PartSO[] availableParts;
    public ModuleData[] availableModules;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void CreateMecha()
    {
        GameObject mecha = Instantiate(mechaPrefab);
        
        foreach(GameObject holder in mechaPartsHolders)
        {
            foreach (PartSO part in currentCoreData.parts)
            {
                if (part is HeadSO)
                {
                    GameObject partObj = Instantiate(part.gameObject, holder.transform);
                    partObj.transform.localPosition = Vector3.zero;
                    partObj.transform.localRotation = Quaternion.identity;
                }
            }
        }

        //MechaController controller = mecha.GetComponent<MechaController>();
        //controller.Initialize();
    }
    */
}
