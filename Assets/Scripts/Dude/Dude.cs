using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Material HightligtMaterial;


    private Renderer _renderer;
    private Material _defaultMaterial;
    private bool _highlighted = false;
    public enum DudeState {
        Idle,
        Walking,
        Grabbed
    }

    public DudeState State;

	// Use this for initialization
	void Start () {
	    _renderer = GetComponent<Renderer>();
	    _defaultMaterial = _renderer.material;

	}
	
	// Update is called once per frame
	void Update () {
	    if (_highlighted && Input.GetMouseButton(0) && !DudeMouseGrab.Instance.Grabbed) {
	        DudeMouseGrab.Instance.Grab(this);
	        State = DudeState.Grabbed;
	        _highlighted = false;
        }
	}

    void OnMouseEnter() {
        if (DudeMouseGrab.Instance.Grabbed) return;
        _renderer.material = HightligtMaterial;
        _highlighted = true;
    }

    void OnMouseExit() {
        if (DudeMouseGrab.Instance.Grabbed) return;
        _renderer.material = _defaultMaterial;
        _highlighted = false;
    }

    public void Release()
    {
        _renderer.material = _defaultMaterial;
        _highlighted = false;
        State = DudeState.Walking;
    }
}
