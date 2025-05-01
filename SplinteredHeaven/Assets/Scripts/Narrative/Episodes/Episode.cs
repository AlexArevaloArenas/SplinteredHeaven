using UnityEngine;

[CreateAssetMenu(fileName = "Episode", menuName = "Narrative/Episode")]
public class Episode : ScriptableObject
{
    string id;
    string scene;
    NarrativeAction[] startActions;
    NarrativeAction[] endActions;
    
}
