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
        Grabbed,
		Melting
    }

    public DudeState State;

	private MovementOnSphere _movement;
	private MeltableBase _target;
	[SerializeField]
	private float _dps = 1;

	void Start () {
	    _renderer = GetComponentInChildren<Renderer>();
	    _defaultMaterial = _renderer.material;

		_movement = GetComponent<MovementOnSphere> ();
		_movement.OnReachedTarget.AddListener (Melt);

		FindNewTarget ();
	}

	void Update () {
	    if (_highlighted && Input.GetMouseButton(0) && !DudeMouseGrab.Instance.Grabbed) {
	        DudeMouseGrab.Instance.Grab(this);
	        State = DudeState.Grabbed;
	        _highlighted = false;
        }
		switch (State) {
		    case DudeState.Melting:
			    _target.AddHealth (-_dps);
			    break;
		    default:
			    break;
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
        FindNewTarget();
    }
    
    private void FindNewTarget(){
		_target = MeltableBase.GetClosestMeltable (transform.position);
		State = DudeState.Walking;
		if (_target == null)
			return;

		_movement.SetTarget (_target);
		_target.OnMelted.AddListener (FindNewTarget);
	}

	private void Melt(){
		State = DudeState.Melting;
	}

}
