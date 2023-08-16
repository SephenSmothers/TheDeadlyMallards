using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [Header ("----Camera Settings----")]
    public float sensitivity;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertY;

    float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Getting Mouse Movement
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        //Stopping camera for going too far
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //Rotate camera on x-axis
        transform.localRotation = Quaternion.Euler(xRotation,0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
   
}
