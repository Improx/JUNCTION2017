using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOnSphere : MonoBehaviour {

	[SerializeField]
	private GameObject _planet;
	private float _movementSpeed = 10f;
	[SerializeField]
	private GameObject _target;

	void Update()
	{
		Vector3 axis = Vector3.Cross (_target.transform.position - transform.position, _planet.transform.position - transform.position);
		transform.RotateAround (_planet.transform.position, axis.normalized, _movementSpeed * Time.deltaTime); 
	}

	public void SetTarget(GameObject target)
	{
		_target = target;
	}
}
