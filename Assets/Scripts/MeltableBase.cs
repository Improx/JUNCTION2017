using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeltableBase : MonoBehaviour {

	private float _startingHealth = 100;
	private float _currentHealth;
	private Vector3 _startingScale;
	private MeshFilter _meshFilter;

	[SerializeField]
	private Planet _planet;
	private Vector3 _vectorToPlanet;

	void Start(){
		_currentHealth = _startingHealth;
		_startingScale = transform.localScale;
		_meshFilter = GetComponent<MeshFilter> ();

		_vectorToPlanet = (_planet.transform.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation (-_vectorToPlanet);

	}

	public void AddHealth(float amount){
		_currentHealth += amount;
		UpdateSize ();

		if (_currentHealth <= 0) {
			Die ();
		}
	}

	private void Die(){
		Destroy (gameObject);
	}
		

	private void UpdateSize(){
		Vector3 temp = transform.localScale;
		temp.z = _startingScale.z * (_currentHealth / _startingHealth);
		transform.localScale = temp;
	}

	public float GetRadius(){
		return _meshFilter.mesh.bounds.size.x;
	}
}
