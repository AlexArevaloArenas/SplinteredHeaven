using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(UnitManager))]
public class UnitMovement : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    public GameObject target;

    void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    public void MoveTo(Vector3 position)
    {
        if (destinationSetter.target == null) destinationSetter.target = target.transform;
        destinationSetter.target.position = position;
    }

    public void Stop()
    {
        destinationSetter.target = null;
    }


    private void OnEnable()
    {
        target = new GameObject(gameObject.name + " Target");
        target.transform.position = transform.position; // Set the target to the same position as the character

    }

    private void OnDisable()
    {
        target = null; // Set the target to null when the object is disabled
        Destroy(target);
    }

}
