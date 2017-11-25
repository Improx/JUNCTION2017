using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeViveGrab : MonoBehaviour
{
    public Transform GrabPoint;
    public Animator AnimationController;

    public Dude Grabbed { get; private set; }

    private SteamVR_TrackedController _controller;

    private Dude _lastCollided;
    // Use this for initialization
    void Start ()
	{
	    _controller = GetComponent<SteamVR_TrackedController>() ?? gameObject.AddComponent<SteamVR_TrackedController>();

        _controller.TriggerClicked += _controller_TriggerClicked;
	    _controller.TriggerUnclicked += _controller_TriggerUnClicked;
    }

    private void _controller_TriggerClicked(object sender, ClickedEventArgs e) => Grab(_lastCollided);
    private void _controller_TriggerUnClicked(object sender, ClickedEventArgs e) => Release();

    // Update is called once per frame
    void Update () {

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
        foreach (var col in Grabbed.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        Grabbed.transform.SetParent(null);
        Grabbed.Release();

        AnimationController.SetBool("Grabbed", false);
        Grabbed = null;
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
