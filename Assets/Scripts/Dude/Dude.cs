using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour {

    public enum DudeState
    {
        Idle,
        Walking,
		Melting,
		InAir
    }

    public DudeState State;

	private MovementOnSphere _movement;
	private MeltableBase _target;
	[SerializeField]
	private float _dps = 1;

	void Start () {
		_movement = GetComponent<MovementOnSphere> ();
		_movement.OnReachedTarget.AddListener (Melt);

		FindNewTarget ();
	}

	void Update () {
		switch (State) {
		case DudeState.Melting:
			_target.AddHealth (-_dps);
			break;
		default:
			break;
		}
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
