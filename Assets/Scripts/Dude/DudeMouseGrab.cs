using System;
using UnityEngine;

public class DudeMouseGrab : MonoBehaviour
{
    public Dude Grabbed { get; private set; }

    public static DudeMouseGrab Instance { get; private set; }

	// Use this for initialization
	void Start () {
        if (Instance) throw new Exception("Multiple DudeMouseGrab instances");
	    Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Grabbed) return;
	    if (Grabbed.State != Dude.DudeState.Grabbed) return;
	    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

	    RaycastHit hit;
	    if (!Physics.Raycast(ray, out hit)) return;
	    if (!hit.rigidbody || !hit.rigidbody.GetComponent<Planet>()) return;
	    var planet = hit.rigidbody.GetComponent<Planet>();

        Grabbed.transform.position = hit.point;

	    var vectorFromPlanet =  Grabbed.transform.position - planet.transform.position;
	    Grabbed.transform.up = vectorFromPlanet;

	    if (Input.GetMouseButtonUp(0)) {
	        Release(planet);
	    }
	}

    public bool Grab(Dude dude)
    {
        if (Grabbed) return false;

        Grabbed = dude;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
        return true;
    }

    public void Release(Planet planet)
    {
        if (!Grabbed) return;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>()) {
            col.enabled = true;
        }
        Grabbed.Release();

        Grabbed = null;
    }
}
