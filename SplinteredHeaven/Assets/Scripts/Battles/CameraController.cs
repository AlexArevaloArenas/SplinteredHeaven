using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.SimpleSmoothModifier;
using static UnityEngine.GraphicsBuffer;


public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] float zoomSpeed = 2f;
    //float rotationSpeed = 1f;

    [SerializeField] float maxHeight = 25f;
    [SerializeField] float minHeight = 4f;

    private bool fixPlayerMovement = false;

    float scrollSpeed = 0f;
    public GameObject cameraObj;

    Vector3 currentVerticalSpeed; // Vertical speed for camera movement
    public float verticalSmooth = 0.2f;

    private void Start()
    {
        EventManager.Instance.MouseWheel += ScrollWheel;
        EventManager.Instance.FixPlayerMovementEvent += () => fixPlayerMovement = true;
        EventManager.Instance.FreePlayerMovementEvent += () => fixPlayerMovement = false;
        EventManager.Instance.UnitMovesEvent += CameraShake;

        cameraObj = transform.GetChild(0).gameObject; // Assuming the camera is the first child of this GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if(fixPlayerMovement)
        {
            return; // Prevent camera movement if player movement is fixed
        }

        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal"); //horizontalSpeed
        float vsp = transform.position.y *  speed * Input.GetAxis("Vertical"); //verticalSpeed
        //float sSpeed = Mathf.Log(transform.position.y) *  -zoomSpeed * scrollSpeed;
        //float sSpeed = Mathf.Log(transform.position.y) * zoomSpeed * scrollSpeed;
        float sSpeed = Mathf.Log(transform.position.y) * zoomSpeed * scrollSpeed;

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
        //Vector3 verticalMove = transform.up * sSpeed;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, sSpeed+transform.position.y, transform.position.z), ref currentVerticalSpeed, verticalSmooth);

        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        //Vector3 move = verticalMove + lateralMove + forwardMove;
        Vector3 move = lateralMove + forwardMove;

        transform.position += move;
        //transform.position.y = Mathf.Clamp(minHeight, maxHeight);



    }

    public void ScrollWheel(float sSpeed)
    {
        scrollSpeed = -Mathf.Clamp(sSpeed, -1, 1);
    }

    private void CameraShake()
    {
        //transform.position += Random.insideUnitSphere * 0.1f; // Simple shake effect
        cameraObj.transform.DOShakePosition(0.1f,0.1f,5,90);
    }
}
