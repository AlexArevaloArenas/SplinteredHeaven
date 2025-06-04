using UnityEngine;

public class submeshAdditionalMaterialRenderer : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Mesh mesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mesh.subMeshCount = mesh.subMeshCount + 1;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
