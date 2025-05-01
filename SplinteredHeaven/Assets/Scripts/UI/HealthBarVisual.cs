
using UnityEngine;

public class HealthBarVisual : MonoBehaviour
{
    public UnitManager unidad;
    [SerializeField] Vector3 margen = new Vector2(0, 0);
    Vector3 nuevaPos;
    public Transform myCam;
    private void Start()
    {
        myCam = Camera.main.transform;
    }
    void Update()
    {
        if (!unidad.selected){
            transform.LookAt(myCam);
            nuevaPos = new Vector3(unidad.transform.position.x + margen.x, unidad.transform.position.y + 1000f, unidad.transform.position.z + margen.z);
            transform.position = nuevaPos;
            return;
        }
        transform.LookAt(myCam);
        nuevaPos = new Vector3(unidad.transform.position.x + margen.x, unidad.transform.position.y + margen.y, unidad.transform.position.z + margen.z);
        transform.position = nuevaPos;
    }
}
