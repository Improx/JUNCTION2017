using System;
using UnityEngine;

public class DudeMouseGrab : MonoBehaviour
{
    public float DropDistance = 2f;

    public Dude Grabbed { get; private set; }

    public static DudeMouseGrab Instance { get; private set; }


    private Camera _camera;
	// Use this for initialization
	void Start () {
        if (Instance) throw new Exception("Multiple DudeMouseGrab instances");
	    Instance = this;

	    _camera = GetComponentInChildren<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
	    if (!Grabbed) return;
	    if (Grabbed.State != Dude.DudeState.Grabbed) return;
	    var ray = _camera.ScreenPointToRay(Input.mousePosition);

	    RaycastHit hit;
	    if (Physics.Raycast(ray, out hit)) {
	        if (hit.rigidbody && hit.rigidbody.GetComponent<Planet>()) {
                var planet = hit.rigidbody.GetComponent<Planet>();


	            var vectorFromPlanet = (Grabbed.transform.position - planet.transform.position).normalized;

	            Grabbed.transform.position = hit.point + vectorFromPlanet * DropDistance;
                Grabbed.transform.forward = vectorFromPlanet;

                if (Input.GetMouseButtonUp(0)) {
                    Release(planet);
	            }
	        }
	    }
	}

    public bool Grab(Dude dude) {
        if (Grabbed) return false;

        Grabbed = dude;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>()) {
            col.enabled = false;
        }
        return true;
    }

    public void Release(Planet planet)  {
        if (!Grabbed) return;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>()) {
            col.enabled = true;
        }
        Grabbed.Release(planet);


        Grabbed = null;
    }
}
