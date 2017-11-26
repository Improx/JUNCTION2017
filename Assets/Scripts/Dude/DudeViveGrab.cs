using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeViveGrab : MonoBehaviour
{
    public Transform GrabPoint;
    public Animator AnimationController;
    public float PlanetSnapDistance = 5f;

    public Dude Grabbed { get; private set; }
    public Vector3 Velocity;

    private SteamVR_TrackedController _controller;

    private Dude _lastCollided;

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

    public void Release() {
        if (!Grabbed) return;

        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        if (_closestPlanet) {

            Grabbed.transform.SetParent(_closestPlanet.transform);
            Grabbed.Release(_closestPlanet);

            if (_closestPlanet) {
                _closestPlanet.Highlight.SetActive(false);
                _closestPlanet = null;
            }
        } else {
            Grabbed.transform.SetParent(null);
            Grabbed.Throw(Velocity);
        }

        AnimationController.SetBool("Grabbed", false);
        Grabbed = null;
    }

    private void SetCollidingObject(Collider other) {
        if (Grabbed) return;

        if (other.attachedRigidbody == null) return;
        var dude = other.attachedRigidbody.GetComponent<Dude>();
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
