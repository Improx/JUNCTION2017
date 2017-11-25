using UnityEngine;
using UnityEngine.Events;

public class DudeMovement : MonoBehaviour {

	[SerializeField]
	private Planet _planet;
	[SerializeField]
	private float _movementSpeed = 10f;
	[SerializeField]
	private MeltableBase _target;
	private float _distanceToTarget = 0;

	private float _meshRadius = 0;
    private Dude _dude;

	public UnityEvent OnReachedTarget = new UnityEvent();

	private void Start() {
	    _dude = GetComponent<Dude>();
		_planet = FindObjectOfType<Planet>();
		_meshRadius = _planet.GetComponentInChildren<MeshFilter> ().mesh.bounds.size.x * 0.5f;

		AlignWithPlanet(_planet);
	}

	void Update() {
		MoveToTarget ();
	}

	private void AlignWithPlanet(Planet targetPlanet){
		var _vectorToPlanet = (targetPlanet.transform.position - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation (-_vectorToPlanet, transform.right);
	}

	private void MoveToTarget() {
	    if (_dude.State != Dude.DudeState.Walking) return;
        if (_target == null) {
			return;
		}

		Vector3 axis = Vector3.Cross (_target.transform.position - transform.position, _planet.transform.position - transform.position);
		transform.RotateAround (_planet.transform.position, axis.normalized, _movementSpeed * Time.deltaTime);

		_distanceToTarget = Vector3.Distance (transform.position, _target.transform.position);

		if (_distanceToTarget <= _target.GetRadius() + _meshRadius) {
			_target = null;
			OnReachedTarget.Invoke ();
		}
	}

	public void SetTarget(MeltableBase target){
		_target = target;
	}
}
