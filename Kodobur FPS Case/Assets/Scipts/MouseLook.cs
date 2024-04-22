using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        slider.value = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
        if(Input.GetButtonDown("Fire2"))
        {
            Camera.main.fieldOfView = 20f;
        }
        if(Input.GetButtonUp("Fire2"))
        {
            Camera.main.fieldOfView = 60f;
        }
    }

    public void OnValueChange()
    {
        mouseSensitivity = slider.value;
    }
}
