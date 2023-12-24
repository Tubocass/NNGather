using gather;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 20, followSpeed = 5, maxFOV = 30, minFOV = 20, scrollSpeed = 3;
    bool isFollowing = false;
    Camera mainCamera;
    [SerializeField] Vector3 offset;
    Vector3 followPosition;
    Vector3 movement;
    [SerializeField] Transform target;
    Transform camTransform;


    void Start()
    {
        mainCamera = GetComponent<Camera>();
        camTransform = GetComponent<Transform>();
    }

    void Update()
    {
        float lastInputX = Input.GetAxis(Inputs.Horizontal);
        float lastInputY = Input.GetAxis(Inputs.Vertical);
        float lastInputScroll = Input.GetAxis(Inputs.ScrollWheel);
        if (lastInputX != 0f || lastInputY != 0f)
        {
            //movement = new Vector3(speed * lastInputX, 0, speed * lastInputY);
            movement.x = moveSpeed * lastInputX;
            movement.y = moveSpeed * lastInputY;
            MoveTo(movement);
        }

        if (lastInputScroll > 0f || lastInputScroll < 0f)
        {
            mainCamera.orthographicSize -= lastInputScroll * scrollSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minFOV, maxFOV);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetFollow();
        }

    }

    public void SetFollow()
    {
        isFollowing = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        camTransform.position = new Vector3(newTarget.position.x, newTarget.position.y, camTransform.position.z);
        offset = camTransform.position - target.position;
    }

    public void MoveTo(Vector3 position)
    {
        isFollowing = false;
        Vector3 targetDir = camTransform.position + position * Time.deltaTime;
        camTransform.position = Vector3.MoveTowards(camTransform.position, targetDir, 1f);
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            followPosition = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            camTransform.position = Vector3.Lerp(camTransform.position, followPosition, followSpeed * Time.deltaTime);
        }

    }
}
