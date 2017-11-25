using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Experimental.UIElements;
using static UnityEngine.Mathf;

public class EarthMovement : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject Planet;

    public float MinDistance = 5f;
    public float MaxDistance = 100f;
    public float RotSpeed = 50f;
    public float ScrollSpeed = 10;

    private float _planetRadius;

    private float _nextDistance;
    // Use this for initialization
    void Start () {
        MainCamera.transform.LookAt(transform);
        _planetRadius = Planet.GetComponent<SphereCollider>().radius;
        _nextDistance = Min(MaxDistance, Max(MinDistance, Vector3.Distance(MainCamera.transform.position, Planet.transform.position) - _planetRadius));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            var rotX = Input.GetAxis("Mouse X") * RotSpeed * Deg2Rad;
            var rotY = Input.GetAxis("Mouse Y") * RotSpeed * Deg2Rad;
            MainCamera.transform.LookAt(transform);
            transform.Rotate(Vector3.up, rotX, Space.World);

            transform.Rotate(Vector3.right, -rotY);
        }

        var scroll = -Input.GetAxis("Mouse ScrollWheel");


        _nextDistance += scroll * ScrollSpeed;
        _nextDistance = Min(MaxDistance, Max(MinDistance, _nextDistance));
    }

    void FixedUpdate() {
        var vecFromPlanet = (MainCamera.transform.position - Planet.transform.position).normalized;
        var distance = Vector3.Distance(MainCamera.transform.position, Planet.transform.position) - _planetRadius;
        MainCamera.transform.position = Planet.transform.position + vecFromPlanet * (_planetRadius + Min(MaxDistance, Max(MinDistance, distance + (_nextDistance - distance) * Time.deltaTime * ScrollSpeed)));
    }
}
