using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] float zoomSpeed = 2f;
    //float rotationSpeed = 1f;

    [SerializeField] float maxHeight = 25f;
    [SerializeField] float minHeight = 4f;

    float scrollSpeed = 0f;

    private void Start()
    {
        EventManager.Instance.MouseWheel += ScrollWheel;
    }

    // Update is called once per frame
    void Update()
    {
        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal"); //horizontalSpeed
        float vsp = transform.position.y *  speed * Input.GetAxis("Vertical"); //verticalSpeed
        float sSpeed = Mathf.Log(transform.position.y) *  -zoomSpeed * scrollSpeed;

        //Movement Locks
        if (transform.position.y + sSpeed >= maxHeight && sSpeed > 0)
        {
            sSpeed = 0;
        }
        else if (transform.position.y - sSpeed <= minHeight && sSpeed < 0)
        {
            sSpeed = 0;
        }

        if (transform.position.y + sSpeed > maxHeight)
        {
            //sSpeed = maxHeight - sSpeed;
            sSpeed = 0;
        }
        else if (transform.position.y + sSpeed < minHeight)
        {
            //sSpeed = minHeight - sSpeed;
            sSpeed = 0;
        }

        //Movement
        Vector3 verticalMove = new Vector3(0, sSpeed, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = verticalMove + lateralMove + forwardMove;
        transform.position += move;
        //transform.position.y = Mathf.Clamp(minHeight, maxHeight);



    }

    public void ScrollWheel(float sSpeed)
    {
        scrollSpeed = sSpeed;
        Debug.Log(scrollSpeed);
    }
}
