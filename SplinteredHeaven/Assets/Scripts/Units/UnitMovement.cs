using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(UnitManager))]
public class UnitMovement : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    public GameObject target;
    public bool isMoving = false;
    public float stopDistance = 5f; // Distance at which the unit stops moving

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    public bool MoveTo(Vector3 position)
    {
        if (destinationSetter.target == null) destinationSetter.target = target.transform;

        if (Vector3.Distance(transform.position, position)< stopDistance)
        {
            Stop();
            return true; // Stop moving if within stop distance
        }
        else
        {
            isMoving = true;
            destinationSetter.target.position = position;
            return false; // Continue moving towards the position
        }
        
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
