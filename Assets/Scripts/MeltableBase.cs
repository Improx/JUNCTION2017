using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MeltableBase : MonoBehaviour {

    [SerializeField]
    protected float StartingHealth = 100;
    protected float CurrentHealth;
	private Vector3 _startingScale;
	private MeshFilter _meshFilter;

	[SerializeField]
	private Planet _planet;

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
		CurrentHealth = StartingHealth;
		_startingScale = transform.localScale;
		_meshFilter = GetComponentInChildren<MeshFilter> ();

		AlignWithPlanet(_planet);

	    _gameManager = FindObjectOfType<GameManager>();

	}

	private void AlignWithPlanet(Planet targetPlanet){
		_vectorToPlanet = (targetPlanet.transform.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation (-_vectorToPlanet);
	}

	public void AddHealth(float amount){
		CurrentHealth += amount;
		UpdateSize ();

		if (CurrentHealth <= 0) {
			Die ();
		}
	}

	private void Die(){
		MeltableBase.meltables.Remove (this);
		if (MeltableBase.meltables.Count == 0) {
			//_gameManager.EndGame ();
		}
		Destroy (gameObject);
		OnMelted.Invoke ();
	}
		

	private void UpdateSize(){
		var temp = transform.localScale;
		temp.z = _startingScale.z * (CurrentHealth / StartingHealth);
		transform.localScale = temp;
	}

	public float GetRadius(){
		return (_meshFilter.mesh.bounds.size.x * transform.lossyScale.x) * 0.5f;
	}
}
