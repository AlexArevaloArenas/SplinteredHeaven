using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class SceneUIAutoGenerator : EditorWindow
{
    private string uiClassName = "SceneUI";
    private MonoScript managerScript;
    private MonoScript interfaceScript;

    [MenuItem("Tools/Generate Scene UI Class")]
    public static void ShowWindow()
    {
        GetWindow<SceneUIAutoGenerator>("Scene UI Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Scene UI Script", EditorStyles.boldLabel);

        uiClassName = EditorGUILayout.TextField("Class Name", uiClassName);
        managerScript = (MonoScript)EditorGUILayout.ObjectField("Manager Script", managerScript, typeof(MonoScript), false);
        interfaceScript = (MonoScript)EditorGUILayout.ObjectField("UI Interface Script", interfaceScript, typeof(MonoScript), false);

        if (GUILayout.Button("Generate Script"))
        {
            if (ValidateInputs())
            {
                GenerateScript();
            }
            else
            {
                EditorUtility.DisplayDialog("Missing Info", "Please assign a valid manager and interface script.", "OK");
            }
        }
    }

    private bool ValidateInputs()
    {
        return managerScript != null && interfaceScript != null && !string.IsNullOrWhiteSpace(uiClassName);
    }

    private void GenerateScript()
    {
        string managerType = managerScript.GetClass().Name;
        string interfaceType = interfaceScript.GetClass().Name;

        string code = $@"using UnityEngine;

        public class {uiClassName} : SceneUI<{managerType}, {interfaceType}>, {interfaceType}
        {{
            public override void InitializeUI()
            {{
                // Optional UI setup
            }}

            // Implement {interfaceType} methods below
        }}";

        string path = EditorUtility.SaveFilePanelInProject("Save UI Script", uiClassName, "cs", "Choose location to save the generated UI script.");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, code);
            AssetDatabase.Refresh();
        }
    }
}
