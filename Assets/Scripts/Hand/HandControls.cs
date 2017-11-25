using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

	private Animator _handAnimator;

	// Use this for initialization
	void Start () {
		_handAnimator = GetComponent<Animator> ();
		_handAnimator.SetBool ("Grabbing", false); 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			_handAnimator.SetBool ("Grabbing", true); 
		} 
		else if (Input.GetMouseButtonUp (0)) {
			_handAnimator.SetBool ("Grabbing", false); 
		}
	}
}
