using UnityEngine;

[CreateAssetMenu(fileName = "SOEpisode", menuName = "Scriptable Objects/SOEpisode")]
public class SOEpisode : ScriptableObject
{
    public string id;
    public SOEpisode[] nextAvailableEpisodes;
}
