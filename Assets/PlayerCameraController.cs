using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Mouse settings")] 
    public float sensitivity;
    public float smoothing;
    [Range(50, 100)]
    public float fieldOfView = 70;
    public Vector2 verticalClamp;

    [Header("Booleans")] 
    public bool mouseIsLocked;

    [Header("Input Variables")] 
    public float mouseX;
    public float mouseY;

    private float rotationX;
    private float rotationY;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeFov();
        inputHandler();
        if(mouseIsLocked)
            moveHandler();
    }

    public void moveHandler()
    {
        rotationY = mouseX * sensitivity;
        rotationX -= mouseY * sensitivity;

        rotationX = Mathf.Clamp(rotationX, verticalClamp.x, verticalClamp.y);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.parent.rotation *= Quaternion.Euler(0, rotationY, 0);
    }



    public void inputHandler()
    {
        mouseX = Mathf.Lerp(mouseX, Input.GetAxisRaw("Mouse X"), smoothing * Time.deltaTime);
        mouseY = Mathf.Lerp(mouseY, Input.GetAxisRaw("Mouse Y"), smoothing * Time.deltaTime);

        if (mouseIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ChangeFov()
    {
        GetComponent<Camera>().fieldOfView = fieldOfView;
    }
}

