using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour {

	private float _distanceFromCamera;

	void Start(){
		_distanceFromCamera = transform.position.z;
	}

	void Update(){
		
		var mousePos = Input.mousePosition;
		mousePos.z = 10;
		transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}
}
