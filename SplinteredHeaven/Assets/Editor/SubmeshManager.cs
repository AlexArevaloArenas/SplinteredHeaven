using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class SubmeshManager : EditorWindow
{
    
    Mesh mesh;
    int[] triangles;
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
            //if (GUILayout.Button("View Info")) ViewInfo();
        }

        GUILayout.Label("Submesh Count: " + mesh.subMeshCount);
        for (int i = 0; i< mesh.subMeshCount; i++)
        {
            triangles = mesh.GetTriangles(i);
            GUILayout.Label($"Submesh {i} has {triangles.Length / 3} triangles.");
        }
    }

    /*void ViewInfo()
    {
        Debug.Log("Submesh Count: " + mesh.subMeshCount);
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            triangles = mesh.GetTriangles(i);
            Debug.Log($"Submesh {i} has {triangles.Length / 3} triangles.");
        }
    }*/

    void GenerateSubMesh(){
        mesh.subMeshCount = mesh.subMeshCount + 1;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    void RemoveSubMesh()
    {
        mesh.subMeshCount--;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount);
    }
}
