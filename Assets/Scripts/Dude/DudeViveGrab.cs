using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DudeViveGrab : MonoBehaviour
{
    public Transform GrabPoint;
    public Animator AnimationController;
    public float PlanetSnapDistance = 5f;

    public Dude Grabbed { get; private set; }

    private SteamVR_TrackedController _controller;

    private Dude _lastCollided;

    private Planet _closestPlanet;
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

    // Update is called once per frame
    void LateUpdate () {
        if (!Grabbed) return;

        _closestPlanet = null;
        foreach (var planet in FindObjectsOfType<Planet>()) {
            var distance = Vector3.Distance(transform.position, planet.transform.position) - planet.Radius;

            if (!(distance < PlanetSnapDistance)) continue;

            if (_closestPlanet != null) {
                var oldDistance = Vector3.Distance(transform.position, _closestPlanet.transform.position) - _closestPlanet.Radius;

                if (oldDistance > distance) continue;
            }

            _closestPlanet = planet;
        }

        if (_closestPlanet != null) {
            _closestPlanet.Highlight.SetActive(true);
        }
    }

    public bool Grab(Dude dude)
    {
        if (Grabbed || !dude) return false;

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

    public void Release()
    {
        if (!Grabbed) return;
        if (_closestPlanet)
        {
            foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }

            Grabbed.transform.SetParent(_closestPlanet.transform);
            Grabbed.Release(_closestPlanet);

            AnimationController.SetBool("Grabbed", false);
            Grabbed = null;

            if (_closestPlanet)
            {
                _closestPlanet.Highlight.SetActive(false);
                _closestPlanet = null;
            }
        }
    }

    private void SetCollidingObject(Collider other) {
        if (Grabbed) return;

        if (other.attachedRigidbody == null) return;
        var dude = other.attachedRigidbody.GetComponent<Dude>();
        if (!dude) return;
        print(dude);
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
