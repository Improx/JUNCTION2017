using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour {

	void Update(){
		
		var mousePos = Input.mousePosition;
		mousePos.z = 10;
		transform.position = Camera.main.ScreenToWorldPoint(mousePos);

	}
}
