using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeltableBase : MonoBehaviour {

	private float _startingHealth = 100;
	private float _currentHealth;
	private Vector3 _startingScale;
	private MeshFilter _meshFilter;

	void Start(){
		_currentHealth = _startingHealth;
		_startingScale = transform.localScale;
		_meshFilter = GetComponent<MeshFilter> ();
	}

	public void AddHealth(float amount){
		_currentHealth += amount;
		UpdateSize ();
	}

	private void UpdateSize(){
		Vector3 temp = transform.localScale;
		temp.y = _startingScale.y * (_currentHealth / _startingHealth);
		transform.localScale = temp;
	}

	public float GetRadius(){
		return _meshFilter.mesh.bounds.size.x;
	}
}
