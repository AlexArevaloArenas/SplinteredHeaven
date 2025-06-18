using Pathfinding;
using UnityEngine;

public class PartAnimationManager : MonoBehaviour
{
    private Animator animator;
    private PartVisualHandler partVisualHandler;
    private CharacterController characterController;
    private UnitMovement unitMovement;
    //private AIPath aiPath;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        partVisualHandler = GetComponent<PartVisualHandler>();
        Debug.Log(gameObject.transform.root.gameObject.name);
        //aiPath = gameObject.transform.root.gameObject.GetComponent<AIPath>();
        
    }

    private void Start()
    {
        characterController = partVisualHandler.linkedPart.owner.obj.GetComponent<CharacterController>();
        unitMovement = partVisualHandler.linkedPart.owner.obj.GetComponent<UnitMovement>();
    }

    private void Update()
    {
        if(characterController != null)
        {
            SetAnimationFloat("Speed", characterController.velocity.magnitude);
            //Cambiar la velocidad de las animaciones según el characterController.velocity.magnitude (1 = velocidad max, 0 es velocidad min)
            //SetAnimationFloat("AnimationSpeed", (characterController.velocity.magnitude-0) /(aiPath.maxSpeed));
            SetAnimationBool("Moving", unitMovement.isMoving);

       

        }
        SetAnimationBool("Moving", unitMovement.isMoving);

    }

    public void SetAnimationFloat(string variable, float f)
    {
        if (animator != null)
        {
            animator.SetFloat(variable, f); // Example: Set speed to 1.0
        }
        else
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }

    }

    public void SetAnimationBool(string variable, bool b)
    {
        if (animator != null)
        {
            animator.SetBool(variable, b);
        }
        else
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }
    }

    public void SetAnimationInt(string variable, int i)
    {
        if (animator != null)
        {
            animator.SetInteger(variable, i);
        }
        else
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }
    }

    public void SetAnimationTrigger(string variable)
    {
        if (animator != null)
        {
            animator.SetTrigger(variable);
        }
        else
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationName);
        }
        else
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }
    }
}
