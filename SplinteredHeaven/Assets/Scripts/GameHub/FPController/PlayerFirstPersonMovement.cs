using UnityEngine;

public class PlayerFirstPersonMovement : MonoBehaviour
{
    //Speed and gravity
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    //Camera movement
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    //Character controller
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Input Variables
    private bool jump = false;
    private float moveVerticalAxis=0;
    private float moveHorizontalAxis = 0;

    private float mouseX = 0;
    private float mouseY = 0;

    [HideInInspector]
    public bool canMove = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Subscribe Functions To Events
        EventManager.Instance.JumpEvent += JumpInput;
        EventManager.Instance.MoveEvent += MoveInputEvent;
        EventManager.Instance.LookEvent += LookInputEvent;
        EventManager.Instance.FPDialogueEvent += EnterDialogueMode;
        EventManager.Instance.EndFPDialogueEvent += ExitDialogueMode;
        
    }

    // Update is called once per frame
    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * moveVerticalAxis : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * moveHorizontalAxis : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (jump && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            jump = false;
        }
        else
        {
            moveDirection.y = movementDirectionY;
            jump = false;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -mouseY * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX * lookSpeed, 0);

            /* OLD INPUT
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            */
        }
    }

    //Event System Comunication
    private void JumpInput()
    {
        jump = true;
    }

    private void MoveInputEvent(Vector2 input)
    {
        moveVerticalAxis = input.y;
        moveHorizontalAxis = input.x;
    }

    private void LookInputEvent(Vector2 input)
    {
        mouseX = input.x;
        mouseY = input.y;
    }

    private void OnDisable()
    {
        EventManager.Instance.JumpEvent -= JumpInput;
        EventManager.Instance.MoveEvent -= MoveInputEvent;
        EventManager.Instance.LookEvent -= LookInputEvent;
    }

    private void EnterDialogueMode(TextAsset json)
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canMove = false;
    }

    private void ExitDialogueMode()
    {
        canMove = true;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
