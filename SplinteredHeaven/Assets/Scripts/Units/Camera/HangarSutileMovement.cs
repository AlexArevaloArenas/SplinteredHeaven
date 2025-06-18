using UnityEngine;
using UnityEngine.InputSystem;

public class HangarSutileMovement : MonoBehaviour
{
    Camera mainCamera;
    public float lookSpeed = 1f; // Speed of the camera movement

    float rotationX = 0;
    float rotationY = 0;
    private float mouseX = 0;
    private float mouseY = 0;

    public float lookLimit = 35.0f;

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Confined; // Lock cursor to the game window
        //Subscribe Functions To Events
        EventManager.Instance.LookEvent += LookInputEvent;

    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * lookSpeed;
        rotationY += mouseX * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit);
        rotationY = Mathf.Clamp(rotationY, -lookLimit, lookLimit);

        // Apply rotation with NO roll (Z = 0)
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        //transform.rotation = targetRotation; // or smooth it with Slerp if you want

        // Optional: Slerp for smoother camera
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
        



        /*
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 cameraDirection = hit.point-Camera.main.transform.position;
            //cameraDirection.z = 0; // Keep the movement on the horizontal plane
            cameraDirection.Normalize(); // Normalize the direction vector to ensure consistent speed

            Quaternion newRotation = Quaternion.LookRotation(cameraDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * speed);
            Vector3 euler = transform.eulerAngles;
            euler.x = Mathf.Clamp(euler.x, -10, 10);
            euler.y = Mathf.Clamp(euler.x, -10, 10);
            //euler.z = Mathf.Clamp(euler.x, -10, 10);
            transform.rotation = Quaternion.Euler(euler);
        
        }
        */
    }

    private void LookInputEvent(Vector2 input)
    {
        mouseX = input.x;
        mouseY = input.y;
    }
}
