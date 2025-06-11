using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    public bool restrictedCamera = false; // If true, camera rotation is restricted to a certain angle
    public bool fixedCamera = false; // If true, camera rotation is restricted to a certain angle


    //Character controller
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //Fix camera
    private Quaternion lastCameraRotation = Quaternion.identity;
    private Quaternion lastModelRotation = Quaternion.identity;

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

        // Player and Camera rotation
        if (canMove)
        {
            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            /* OLD INPUT
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            */
        }
        if (!fixedCamera)
        {
            rotationX += -mouseY * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            if (!restrictedCamera)
            {
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            }
            transform.rotation *= Quaternion.Euler(0, mouseX * lookSpeed, 0);
        }
        if (restrictedCamera)
        {
            // Restrict camera rotation to a certain angle
            Quaternion targetRotationModel = Quaternion.Euler(0, lastModelRotation.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationModel, Time.deltaTime * lookSpeed *10);

            Quaternion targetRotationCamera = Quaternion.Euler(lastCameraRotation.x, 0, 0f);
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, targetRotationCamera, Time.deltaTime * lookSpeed * 10);
        }
        else
        {
            if(!fixedCamera || !restrictedCamera)
            {
                lastCameraRotation = playerCamera.transform.localRotation;
                lastModelRotation = transform.rotation;
            }
            
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
        EventManager.Instance.FPDialogueEvent -= EnterDialogueMode;
        EventManager.Instance.EndFPDialogueEvent -= ExitDialogueMode;
    }
    
    public IEnumerator LookAtCorrutine(Vector3 pos)
    {
        Quaternion loo = Quaternion.LookRotation(pos);
        while (transform.rotation != Quaternion.Euler(0, loo.eulerAngles.y, 0) && playerCamera.transform.rotation != Quaternion.Euler(loo.eulerAngles.x, 0, 0))
        {
            // Set the rotation of the player model to look at the position
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, loo.eulerAngles.y, 0), Time.deltaTime * lookSpeed * 6);
            // Set the rotation of the camera to look at the position
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(loo.eulerAngles.x, 0, 0), Time.deltaTime * lookSpeed * 6);
            yield return new WaitForSeconds(0.01f);

        }
        // After the rotation is complete, set fixedCamera to false to allow further rotations
        //fixedCamera = false; // Reset fixed camera to allow further rotation changes
        lastCameraRotation = playerCamera.transform.localRotation; // Update last camera rotation
        lastModelRotation = transform.rotation; // Update last model rotation

    }
    

    public void LookAt(Vector3 pos)
    {
        if (restrictedCamera || fixedCamera) return; // Do not allow looking at a position if camera is restricted or fixed
        
        fixedCamera = true; // Set fixed camera to true to prevent further rotation changes


        //Quaternion loo = Quaternion.LookRotation(pos);
        //transform.rotation = Quaternion.Euler(0, loo.eulerAngles.y, 0);
        //playerCamera.transform.rotation = Quaternion.Euler(loo.eulerAngles.x, 0, 0);

        StartCoroutine(LookAtCorrutine(pos)); // Start the coroutine to look at the position
    }

    private void EnterDialogueMode(TextAsset json,Vector3 pos)
    {
        // Lock cursor
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        LookAt(pos); // Look at the dialogue position
        canMove = false;
        fixedCamera = true; // Lock camera rotation

        //lastCameraRotation = playerCamera.transform.localRotation; // Update last camera rotation
        //lastModelRotation = transform.rotation; // Update last model rotation
    }

    private void ExitDialogueMode()
    {

        // Lock cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        canMove = true;
        fixedCamera = false; // Reset fixed camera to allow further rotation changes
        restrictedCamera = false; // Unlock camera rotation
    }

    private void OnDestroy()
    {
        EventManager.Instance.JumpEvent -= JumpInput;
        EventManager.Instance.MoveEvent -= MoveInputEvent;
        EventManager.Instance.LookEvent -= LookInputEvent;
        EventManager.Instance.FPDialogueEvent -= EnterDialogueMode;
        EventManager.Instance.EndFPDialogueEvent -= ExitDialogueMode;

        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
