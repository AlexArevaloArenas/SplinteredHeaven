using UnityEngine;

public class FirstPersonInteraction : MonoBehaviour
{
    public float interactionDistance = 1f;
    public bool interact = false;
    public bool showing = false;

    public bool block = false;

    private void Start()
    {
        EventManager.Instance.LeftMouseDownEvent += LeftMouseInput;
        EventManager.Instance.BlockInteraction += BlockInteraction; // Subscribe to block interaction event
    }

    private void OnEnable()
    {
        EventManager.Instance.LeftMouseDownEvent += LeftMouseInput;
        EventManager.Instance.BlockInteraction += BlockInteraction; // Subscribe to block interaction event
    }

    private void OnDisable()
    {
        EventManager.Instance.LeftMouseDownEvent -= LeftMouseInput;
        //EventManager.Instance.BlockInteraction -= BlockInteraction; // Subscribe to block interaction event
    }

    private void Update()
    {
        if(block) return; // If interaction is blocked, skip the update
        RaycastHit hit;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactionDistance);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance))
        {
            if (interact) {
                hit.collider.GetComponent<IInteractable>().Interact(gameObject);
            }
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                if (showing ==false)
                {
                    ShowInteractionSimbol(hit.collider.GetComponent<IInteractable>().GetInteractionText());
                    showing = true;
                }  
            }
            else
            {
                if (showing == true)
                {
                    StopInteractionSimbol();
                    showing = false;
                }
            }
        }
        else
        {
            if (showing == true)
            {
                StopInteractionSimbol();
                showing = false;
            }
        }
        interact = false;
    }

    private void LeftMouseInput()
    {
        if (block)
        {
            return;
        }
        interact = true;
    }

    private void ShowInteractionSimbol(string interactionText)
    {
        //Debug.Log("Can interact!");
        EventManager.Instance.StartChangeInteractionSymbol(true, interactionText);
    }

    private void StopInteractionSimbol()
    {
        //Debug.Log("Can not interact!");
        EventManager.Instance.StartChangeInteractionSymbol(false,"");
    }

    public void BlockInteraction(bool _block)
    {
        block = _block;
        EventManager.Instance.LeftMouseDownEvent += LeftMouseInput;
        if (block)
        {
            EventManager.Instance.LeftMouseDownEvent -= LeftMouseInput;
            ShowInteractionSimbol("");
            showing = false;
        }
    }

}
