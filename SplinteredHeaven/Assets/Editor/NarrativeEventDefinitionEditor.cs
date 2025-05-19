using UnityEditor;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(NarrativeEventData))]
public class NarrativeEventDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var current = (NarrativeEventData)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("? Chained Events", EditorStyles.boldLabel);
        foreach (var evt in current.chainedEvents)
        {
            EditorGUILayout.ObjectField(evt.name, evt, typeof(NarrativeEventData), false);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("? Triggered By", EditorStyles.boldLabel);

        var allEvents = AssetDatabase.FindAssets("t:NarrativeEventDefinition");
        foreach (var guid in allEvents)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var otherEvent = AssetDatabase.LoadAssetAtPath<NarrativeEventData>(path);

            if (otherEvent != current && otherEvent.chainedEvents.Contains(current))
            {
                EditorGUILayout.ObjectField(otherEvent.name, otherEvent, typeof(NarrativeEventData), false);
            }
        }
    }
}