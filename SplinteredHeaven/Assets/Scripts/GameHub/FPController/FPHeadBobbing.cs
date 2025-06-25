using UnityEngine;

public class FPHeadBobbing : MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float runningBobbingSpeed = 24f;
    private float bobbingSpeed = 0f;
    private float bobbingAmount = 0f;
    public float walkingbobbingAmount = 0.05f;
    public float runningbobbingAmount = 0.2f;
    public PlayerFirstPersonMovement controller;



    float defaultPosY = 0;
    float timer = 0;
    public bool isBlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
        EventManager.Instance.FPDialogueEvent += Block;
        EventManager.Instance.EndFPDialogueEvent += Unblock;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.jump) return;
        if (controller.isRunning)
        {
            bobbingSpeed = runningBobbingSpeed;
            bobbingAmount = runningbobbingAmount;
        }
        else
        {
            bobbingSpeed = walkingBobbingSpeed;
            bobbingAmount = walkingbobbingAmount;
        }

        if (isBlocked) return;

        if (controller.characterController.velocity.magnitude != 0)
        {
            //Player is moving
            timer += Time.deltaTime * bobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
        }

    }

    public void Block(TextAsset x, Vector3 y)
    {
        isBlocked = true;
    }

    public void Unblock()
    {
        isBlocked = false;
        transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY, transform.localPosition.z);
    }


}
