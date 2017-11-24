using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeMouseGrab : MonoBehaviour
{

    public GameObject Planet;
    public Dude Grabbed { get; private set; }

    public static DudeMouseGrab Instance { get; private set; }

	// Use this for initialization
	void Start () {
        if (Instance) throw new Exception("Multiple DudeMouseGrab instances");
	    Instance = this;
	}
	
	// Update is called once per frame
	void Update () {

	    if (Grabbed) {
	        if (Grabbed.State == Dude.DudeState.Grabbed) {
	            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

	            RaycastHit hit;
	            if (Physics.Raycast(ray, out hit)) {
	                if (hit.collider.gameObject == Planet)
	                {
	                    Grabbed.transform.position = hit.point;
                    }
	            }
	        }
	    }

	    if (Input.GetMouseButtonUp(0) && Grabbed) {
	        Release();
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

    public void Release()
    {
        if (!Grabbed) return;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>()) {
            col.enabled = true;
        }
        Grabbed.Release();

        Grabbed = null;
    }
}
