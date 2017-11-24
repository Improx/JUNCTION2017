using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOnSphere : MonoBehaviour {

	[SerializeField]
	private GameObject _planet;
	[SerializeField]
	private float _movementSpeed = 10f;
	[SerializeField]
	private MeltableBase _target;
	private Vector3 _pivotOffset;
	private float _distanceToTarget = 0;

	void Update(){
		MoveToTarget ();
	}

	private void MoveToTarget(){ 
		if (_target == null) {
			return;
		}

		Vector3 axis = Vector3.Cross (_target.transform.position - transform.position, _planet.transform.position - transform.position);
		transform.RotateAround (_planet.transform.position, axis.normalized, _movementSpeed * Time.deltaTime);

		_distanceToTarget = Vector3.Distance (transform.position - _pivotOffset, _target.transform.position);

		if (_distanceToTarget <= _target.GetRadius()) {
			_target = null;
		}
	}

	public void SetTarget(MeltableBase target){
		_target = target;
	}
}
