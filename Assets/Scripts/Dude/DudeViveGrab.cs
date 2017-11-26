﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DudeViveGrab : MonoBehaviour
{
    public Transform GrabPoint;
    public Animator AnimationController;
    public float PlanetSnapDistance = 5f;

    public Grabbable Grabbed { get; private set; }
    public Vector3 Velocity;

    private SteamVR_TrackedController _controller;

    private Grabbable _lastCollided;

    private Planet _closestPlanet;

    private Vector3 _lastPosition;
    // Use this for initialization
    void Start ()
	{
	    _controller = GetComponent<SteamVR_TrackedController>() ?? gameObject.AddComponent<SteamVR_TrackedController>();

        _controller.TriggerClicked += _controller_TriggerClicked;
	    _controller.TriggerUnclicked += _controller_TriggerUnClicked;
    }

	private void _controller_TriggerClicked(object sender, ClickedEventArgs e){
		Grab (_lastCollided);
	}
	private void _controller_TriggerUnClicked(object sender, ClickedEventArgs e) {
		Release ();
	}

    void FixedUpdate() {
        Velocity = (transform.position - _lastPosition) /Time.fixedDeltaTime;

        _lastPosition = transform.position;
    }

    public bool Grab(Grabbable dude)
    {
        if (Grabbed != null || !dude) return false;

        Grabbed = dude;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        AnimationController.SetBool("Grabbed", true);
        Grabbed.Grab();
        Grabbed.transform.position = GrabPoint.position;
        Grabbed.transform.rotation = GrabPoint.rotation;
        Grabbed.transform.SetParent(GrabPoint);

        return true;
    }

    public void Release() {
        if (!Grabbed) return;

        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        
        Grabbed.transform.SetParent(null);
        Grabbed.Throw(Velocity);


        StartCoroutine(KillTimer(Grabbed.gameObject));
        AnimationController.SetBool("Grabbed", false);
        Grabbed = null;
    }
    private IEnumerator KillTimer(GameObject obj)
    {
        yield return new WaitForSeconds(5);
        Destroy(obj);
    }

    private void SetCollidingObject(Collider other) {
        if (Grabbed) return;

        if (other.attachedRigidbody == null) return;
        var dude = other.attachedRigidbody.GetComponent<Grabbable>();
        if (!dude) return;
        _lastCollided = dude;
    }
    
    public void OnTriggerEnter(Collider other) {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (!_lastCollided) return;

        _lastCollided = null;
    }
}
