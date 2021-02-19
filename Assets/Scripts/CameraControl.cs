using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Transform player;
    public const float speed = 10.0f;
    public const float rotateSpeed = 5.0f;
    public float _mouseX;
    public float _mouseY;

    void Start() { }

    void Update()
    {
        // Keyboard
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(x, 0, z);

        //if (Input.GetKey(KeyCode.Q)) { transform.Rotate(0, -25 * Time.deltaTime, 0, Space.Self); }
        //if (Input.GetKey(KeyCode.E)) { transform.Rotate(0, 25 * Time.deltaTime, 0, Space.Self); }
        //if (Input.GetKey(KeyCode.Z)) { transform.Rotate(-25 * Time.deltaTime, 0, 0, Space.Self); }
        //if (Input.GetKey(KeyCode.C)) { transform.Rotate(25 * Time.deltaTime, 0, 0, Space.Self); }
        //if (Input.GetKey(KeyCode.H)) { transform.Translate(0, 5 * Time.deltaTime, 0); }
        //if (Input.GetKey(KeyCode.N)) { transform.Translate(0, -5 * Time.deltaTime, 0); }

        // Mouse
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
        CameraRotate(_mouseX, _mouseY, rotateSpeed);

        // Mouse Scroll
        CameraFOV();
    }

    public void CameraRotate(float _mouseX, float _mouseY, float rotateSpeed)
    {
        if (Input.GetMouseButton(1)) // if click right button
        {
            transform.RotateAround(transform.position, Vector3.up, _mouseX * rotateSpeed);
            transform.RotateAround(transform.position, -transform.right, _mouseY * rotateSpeed);
        }
    }

    public void CameraFOV()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 200;
        transform.Translate(Vector3.forward * wheel);
    }
}

