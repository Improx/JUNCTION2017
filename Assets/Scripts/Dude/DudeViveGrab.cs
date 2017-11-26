using System.Collections;
using System.Collections.Generic;
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

    private List<Vector3> _lastPositions = new List<Vector3> { new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0),};

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
        //Velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        //_lastPosition = transform.position;

        Velocity = (transform.position - _lastPositions[0]) / Time.fixedDeltaTime;
        _lastPositions.RemoveAt(0);
        _lastPositions.Add(transform.position);
    }

    public bool Grab(Grabbable dude)
    {
        AnimationController.SetBool("Grabbed", true);

        if (Grabbed != null || !dude) return false;

        Grabbed = dude;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        Grabbed.Grab();
        Grabbed.transform.position = GrabPoint.position;
        Grabbed.transform.rotation = GrabPoint.rotation;
        Grabbed.transform.SetParent(GrabPoint);

        SteamVR_Controller.Input((int)_controller.controllerIndex).TriggerHapticPulse(3000);
        return true;
    }

    public void Release() {

        AnimationController.SetBool("Grabbed", false);
        
        if (!Grabbed) return;

        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        
        Grabbed.transform.SetParent(null);
        Grabbed.Throw(Velocity);
        Grabbed.Flying = true;

        SteamVR_Controller.Input((int)_controller.controllerIndex).TriggerHapticPulse(3000);
        StartCoroutine(KillTimer(Grabbed.gameObject));
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
