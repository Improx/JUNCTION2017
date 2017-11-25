﻿using UnityEngine;
using UnityEngine.Events;

public class DudeMovement : MonoBehaviour {
	[SerializeField]
	private float _movementSpeed = 10f;
    [SerializeField]
    private float _fallingSpeed = 1f;
    [SerializeField]
	private MeltableBase _target;
	private float _distanceToTarget = 0;
    
    private Dude _dude;

	public UnityEvent OnReachedTarget = new UnityEvent();

    public Planet Planet { get; private set; }

    private void Start() {
	    _dude = GetComponent<Dude>();

		AlignWithPlanet(FindObjectOfType<Planet>());
	}

	private void Update() {
	    DropToPlanet();
	    MoveToTarget();
    }

    private void DropToPlanet() {
        if (_dude.State == Dude.DudeState.Grabbed || !Planet) return;

        var distance = Vector3.Distance(transform.position, Planet.transform.position);

        if (!(distance > Planet.Radius)) return;

        if (_dude.State != Dude.DudeState.Falling)  {
            _dude.SetState(Dude.DudeState.Falling);
        }

        var vectorFromPlanet = (transform.position - Planet.transform.position).normalized;
        
        transform.position = Planet.transform.position + vectorFromPlanet * (distance - Time.deltaTime * _fallingSpeed);
    }

    public void AlignWithPlanet(Planet targetPlanet) {
        Planet = targetPlanet;
        var vectorFromPlanet = (transform.position - Planet.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(vectorFromPlanet, transform.right);
    }

    private void MoveToTarget() {
	    if (_dude.State != Dude.DudeState.Walking) return;
        if (_target == null) {
			return;
		}

		var axis = Vector3.Cross (_target.transform.position - transform.position, Planet.transform.position - transform.position);
		transform.RotateAround (Planet.transform.position, axis.normalized, _movementSpeed * Time.deltaTime);

		_distanceToTarget = Vector3.Distance (transform.position, _target.transform.position);

		if (_distanceToTarget <= _target.GetRadius() + Planet.Radius) {
			_target = null;
			OnReachedTarget.Invoke ();
		}
	}

	public void SetTarget(MeltableBase target){
		_target = target;
	}
}
