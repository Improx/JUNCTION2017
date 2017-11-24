using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class EarthMovement : MonoBehaviour
{
    public Camera MainCamera;


    public float RotSpeed = 50;
    // Use this for initialization
    void Start () {
        MainCamera.transform.LookAt(transform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            var rotX = Input.GetAxis("Mouse X") * RotSpeed * Mathf.Deg2Rad;
            var rotY = Input.GetAxis("Mouse Y") * RotSpeed * Mathf.Deg2Rad;
            transform.Rotate(Vector3.up, rotX, Space.World);

            transform.Rotate(Vector3.right, -rotY);
        }
    }
}
