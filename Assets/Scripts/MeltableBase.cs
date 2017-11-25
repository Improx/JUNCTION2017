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
	[SerializeField]
	private GameManager _gameManager;

	private Vector3 _vectorToPlanet;

	public UnityEvent OnMelted = new UnityEvent();

	public static List<MeltableBase> meltables = new List<MeltableBase>();

	public static MeltableBase GetClosestMeltable(Vector3 position){

		var closestDistance = Mathf.Infinity;
		MeltableBase closestMeltable = null;

		foreach (var meltable in MeltableBase.meltables) {
			var distance = Vector3.Distance (position, meltable.transform.position);
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
		_meshFilter = GetComponentInChildren<MeshFilter> ();

		AlignWithPlanet(_planet);
	}

	private void AlignWithPlanet(Planet targetPlanet){
		_vectorToPlanet = (targetPlanet.transform.position - transform.position).normalized;
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
		if (MeltableBase.meltables.Count == 0) {
			_gameManager.EndGame ();
		}
		Destroy (gameObject);
		OnMelted.Invoke ();
	}
		

	private void UpdateSize(){
		var temp = transform.localScale;
		temp.z = _startingScale.z * (_currentHealth / _startingHealth);
		transform.localScale = temp;
	}

	public float GetRadius(){
		return (_meshFilter.mesh.bounds.size.x * transform.lossyScale.x) * 0.5f;
	}
}
