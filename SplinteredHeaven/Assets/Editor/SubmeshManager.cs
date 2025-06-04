using UnityEditor;
using UnityEngine;

public class SubmeshManager : EditorWindow
{
    
    Mesh mesh;
    [MenuItem("Tools/Submesh Manager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SubmeshManager));
    }

    private void OnGUI()
    {
        GUILayout.Label("Add or remove submeshes", EditorStyles.boldLabel);

        mesh = EditorGUILayout.ObjectField(mesh, typeof(Mesh), false) as Mesh;
        if (mesh != null)
        {
            Debug.Log("Mesh: " + mesh.name);
            if (GUILayout.Button("Add New Submesh")) GenerateSubMesh();
            if (GUILayout.Button("Remove Last Submesh")) RemoveSubMesh();
        }
    }

    void GenerateSubMesh(){
        mesh.subMeshCount = mesh.subMeshCount + 1;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    void RemoveSubMesh()
    {
        mesh.subMeshCount = mesh.subMeshCount - 1;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }
}
