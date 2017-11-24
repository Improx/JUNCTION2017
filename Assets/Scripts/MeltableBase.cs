using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MeltableBase : MonoBehaviour {

	private float _startingHealth = 100;
	private float _currentHealth;
	private Vector3 _startingScale;
	private MeshFilter _meshFilter;

	[SerializeField]
	private Planet _planet;
	private Vector3 _vectorToPlanet;

	public UnityEvent OnMelted = new UnityEvent();

	public static List<MeltableBase> meltables = new List<MeltableBase>();

	public static MeltableBase GetClosestMeltable(Vector3 position){

		float closestDistance = Mathf.Infinity;
		MeltableBase closestMeltable = null;

		foreach (var meltable in MeltableBase.meltables) {
			float distance = Vector3.Distance (position, meltable.transform.position);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestMeltable = meltable;
			}
		}

		return closestMeltable;
	}

	void Awake(){
		meltables.Add (this);
	}

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
		MeltableBase.meltables.Remove (this);
		Destroy (gameObject);
		OnMelted.Invoke ();
	}
		

	private void UpdateSize(){
		Vector3 temp = transform.localScale;
		temp.z = _startingScale.z * (_currentHealth / _startingHealth);
		transform.localScale = temp;
	}

	public float GetRadius(){
		return (_meshFilter.mesh.bounds.size.x * transform.lossyScale.x) * 0.5f;
	}
}
