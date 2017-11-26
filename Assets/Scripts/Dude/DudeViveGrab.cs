using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeViveGrab : MonoBehaviour
{
    public Transform GrabPoint;
    public Animator AnimationController;
    public float PlanetSnapDistance = 5f;

    public Grabbable Grabbed { get; private set; }
    public Vector3 Velocity { get; private set; }

    private SteamVR_TrackedController _controller;

    private Grabbable _lastCollided;

    private Planet _closestPlanet;

    private Vector3 _lastPosition;

    // Use this for initialization
    void Start () {
        _lastPosition = transform.position;
        _controller = GetComponent<SteamVR_TrackedController>() ?? gameObject.AddComponent<SteamVR_TrackedController>();

        _controller.TriggerClicked += _controller_TriggerClicked;
	    _controller.TriggerUnclicked += _controller_TriggerUnClicked;
        _controller.MenuButtonClicked += _controller_MenuClicked;
        _controller.PadClicked += _controller_PadClicked;
    }

    private void _controller_PadClicked(object sender, ClickedEventArgs e) {
        if (GameManager.GameStart) return;

        GameManager.StartGame();
    }

    private void _controller_MenuClicked(object sender, ClickedEventArgs e) {
        if (!GameManager.GameOver) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void _controller_TriggerClicked(object sender, ClickedEventArgs e){
		Grab (_lastCollided);
	}
	private void _controller_TriggerUnClicked(object sender, ClickedEventArgs e) {
		Release ();
	}

    void Update() {

        if (!GameManager.GameStart || GameManager.GameOver)
        {
            Release();
            return;
        }

        //Velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;
        //_lastPosition = transform.position;

        var direction =  transform.position - _lastPosition;
        var speed = direction.magnitude / Time.deltaTime;

        Velocity = direction.normalized * speed;
        _lastPosition = transform.position;
    }

    public bool Grab(Grabbable dude)
    {
        if (!GameManager.GameStart || GameManager.GameOver) return false;
        AnimationController.SetBool("Grabbed", true);

        if (Grabbed != null || !dude) return false;

        Grabbed = dude;
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        Grabbed.transform.position = GrabPoint.position;
        Grabbed.transform.rotation = GrabPoint.rotation;
        Grabbed.transform.SetParent(GrabPoint);
        Grabbed.Grab();

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
