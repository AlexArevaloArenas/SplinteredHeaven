using UnityEngine;
using UnityUtils;


public class TreeLayerFix : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetLayersRecursively(LayerMask.NameToLayer("Obstacle"));
    }
}
