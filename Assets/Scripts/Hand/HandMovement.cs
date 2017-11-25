using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{

    private Camera _camera;

    void Start() {
        _camera = GetComponentInParent<Camera>();
    }

	void Update(){
		
		var mousePos = Input.mousePosition;
		mousePos.z = 10;
		transform.position = _camera.ScreenToWorldPoint(mousePos);

	}
}
