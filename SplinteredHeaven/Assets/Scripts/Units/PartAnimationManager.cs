using UnityEngine;

public class PartAnimationManager : MonoBehaviour
{
    private Animator animator;
    private PartVisualHandler partVisualHandler;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        partVisualHandler = GetComponent<PartVisualHandler>();
    }

    private void Update()
    {
        SetAnimationFloat("Speed", partVisualHandler.linkedPart.owner.obj.GetComponent<CharacterController>().velocity.magnitude); // Example: Set speed to 1.0

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
