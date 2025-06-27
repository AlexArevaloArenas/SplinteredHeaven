using UnityEngine;
using UnityEngine.InputSystem;

public class HangarSutileMovement : MonoBehaviour
{
    Camera mainCamera;
    public float lookSpeed = 1f; // Speed of the camera movement

    private float originalX = 0;
    private float originalY = 0;

    float rotationX = 0;
    float rotationY = 0;
    private float mouseX = 0;
    private float mouseY = 0;

    //public float lookLimit = 35.0f+180;
    public float lookLimit = 35.0f;

    private void Start()
    {
        originalX = transform.rotation.eulerAngles.x;
        originalY = transform.rotation.eulerAngles.y;
        lookLimit = transform.rotation.y + lookLimit; // Set the look limit based on the camera's position
        //mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Lock cursor to the game window
        //Subscribe Functions To Events
        EventManager.Instance.LookEvent += LookInputEvent;

    }
    // Update is called once per frame
    void Update()
    {
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * lookSpeed;
        rotationY += mouseX * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit);
        rotationY = Mathf.Clamp(rotationY, -lookLimit, lookLimit);

        // Apply rotation with NO roll (Z = 0)
        Quaternion targetRotation = Quaternion.Euler(rotationX+ originalX, rotationY + originalY, 0f);
        //transform.rotation = targetRotation; // or smooth it with Slerp if you want

        // Optional: Slerp for smoother camera
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
        
    }
    private void OnDisable()
    {
        EventManager.Instance.LookEvent -= LookInputEvent;
    }
    private void LookInputEvent(Vector2 input)
    {
        mouseX = input.x;
        mouseY = input.y;
    }
}
