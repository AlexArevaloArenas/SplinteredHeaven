using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(UnitManager))]
public class UnitMovement : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    public GameObject target;
    public bool isMoving = false;
    void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    public void MoveTo(Vector3 position)
    {
        if (destinationSetter.target == null) destinationSetter.target = target.transform;
        destinationSetter.target.position = position;

        //isMoving = true;
    }

    public void SetTarget(GameObject target)
    {
        if (destinationSetter.target == null) destinationSetter.target = target.transform; 

        else destinationSetter.target.position = target.transform.position;
        
        isMoving = true;
    }

    public void Stop()
    {
        destinationSetter.target = null;
        isMoving = false;

    }


    private void OnEnable()
    {
        target = new GameObject(gameObject.name + " Target");
        target.transform.position = transform.position; // Set the target to the same position as the character

    }

    private void OnDisable()
    {
        Destroy(target);
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            Destroy(target);
        }
    }

}
