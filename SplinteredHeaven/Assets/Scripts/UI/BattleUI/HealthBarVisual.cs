
using UnityEngine;
using UnityEngine.UI;

public class HealthBarVisual : MonoBehaviour
{
    public UnitManager unidad;
    [SerializeField] Vector3 margen = new Vector2(0, 0);
    Vector3 nuevaPos;
    public Transform myCam;
    private Transform heigh;
    public UnityEngine.UI.Image img;
    public Color enemy;
    public Color ally;

    private void Start()
    {
        img = GetComponent<UnityEngine.UI.Image>();
        myCam = Camera.main.transform;
        foreach(var part in unidad.unit.Parts)
        {
            if(part.data.partType==PartType.Head)
            {
                heigh = part.transform;
                break;
            }
        }
        if(heigh == null)
        {
            heigh = unidad.transform;
        }
        if (unidad.gameObject.tag == "Enemy")
        {
            img.color = enemy;
        }
        else
        {
            img.color = ally;
        }
    }
    void Update()
    {
        /*
        if (!unidad.selected){
            transform.LookAt(myCam);
            nuevaPos = new Vector3(unidad.transform.position.x + margen.x, unidad.transform.position.y + 1000f, unidad.transform.position.z + margen.z);
            transform.position = nuevaPos;
            return;
        }
        */
        if (unidad == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.LookAt(myCam);
            nuevaPos = new Vector3(unidad.transform.position.x + margen.x, heigh.position.y + margen.y, unidad.transform.position.z + margen.z);
            transform.position = nuevaPos;
        }

            
    }
}
