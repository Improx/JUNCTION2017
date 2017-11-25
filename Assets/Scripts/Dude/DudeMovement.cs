using UnityEngine;
using UnityEngine.Events;

public class DudeMovement : MonoBehaviour {
	[SerializeField]
	private float _movementSpeed = 10f;
	[SerializeField]
	private MeltableBase _target;
	private float _distanceToTarget = 0;

	private float _meshRadius = 0;
    private Dude _dude;

	public UnityEvent OnReachedTarget = new UnityEvent();

    public Planet Planet { get; private set; }

    private void Start() {
	    _dude = GetComponent<Dude>();

		AlignWithPlanet(FindObjectOfType<Planet>());
	}

	void Update() {
		MoveToTarget ();
    }

    public void AlignWithPlanet(Planet targetPlanet) {
        Planet = targetPlanet;
        _meshRadius = Planet.GetComponentInChildren<MeshFilter>().mesh.bounds.size.x * 0.5f;
        var vectorToPlanet = (targetPlanet.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-vectorToPlanet, transform.right);
    }

    private void MoveToTarget() {
	    if (_dude.State != Dude.DudeState.Walking) return;
        if (_target == null) {
			return;
		}

		Vector3 axis = Vector3.Cross (_target.transform.position - transform.position, Planet.transform.position - transform.position);
		transform.RotateAround (Planet.transform.position, axis.normalized, _movementSpeed * Time.deltaTime);

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
