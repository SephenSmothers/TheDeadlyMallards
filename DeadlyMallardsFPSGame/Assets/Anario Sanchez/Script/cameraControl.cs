using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [SerializeField] int sensitivity;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    public Transform orientation;

    [SerializeField] bool invertY;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Getting Mouse Movement
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            xRotation += mouseY;
            yRotation -= mouseX;
        }
        else
        {
            xRotation -= mouseY;
            yRotation += mouseX;
        }

        //Stopping camera for going too far
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //Rotate camera on x-axis
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
