using UnityEngine;

public class EarthMovement : MonoBehaviour
{
    public Camera MainCamera;
    public Planet Planet;

    public float MinDistance = 5f;
    public float MaxDistance = 100f;
    public float RotSpeed = 50f;
    public float ScrollSpeed = 10;

    private float _planetRadius;

    private float _nextDistance;
    // Use this for initialization
    void Start () {
        MainCamera.transform.LookAt(transform);
        _planetRadius = Planet.Radius;
        _nextDistance = Mathf.Min(MaxDistance, Mathf.Max(MinDistance, Vector3.Distance(MainCamera.transform.position, Planet.transform.position) - _planetRadius));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(1))
        {
            var rotX = Input.GetAxis("Mouse X") * RotSpeed * Mathf.Deg2Rad;
            var rotY = Input.GetAxis("Mouse Y") * RotSpeed * Mathf.Deg2Rad;
            MainCamera.transform.LookAt(transform);
            transform.Rotate(Vector3.up, rotX, Space.World);

            transform.Rotate(Vector3.right, -rotY);
        }

        var scroll = -Input.GetAxis("Mouse ScrollWheel");


        _nextDistance += scroll * ScrollSpeed;
        _nextDistance = Mathf.Min(MaxDistance, Mathf.Max(MinDistance, _nextDistance));
    }

    void FixedUpdate() {
        var vecFromPlanet = (MainCamera.transform.position - Planet.transform.position).normalized;
        var distance = Vector3.Distance(MainCamera.transform.position, Planet.transform.position) - _planetRadius;
        MainCamera.transform.position = Planet.transform.position + vecFromPlanet * (_planetRadius + Mathf.Min(MaxDistance, Mathf.Max(MinDistance, distance + (_nextDistance - distance) * Time.deltaTime * ScrollSpeed)));
    }
}
