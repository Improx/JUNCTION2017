using UnityEngine;

public class Dude : MonoBehaviour
{
    public Material HightligtMaterial;


    //private Renderer _renderer;
    //private Material _defaultMaterial;
    private bool _highlighted = false;
    public enum DudeState {
        Idle,
        Walking,
        Grabbed,
		Melting,
        Falling
    }

    public DudeState State { get; private set; }

	private DudeMovement _movement;
    private MeltableBase _target;
    private Animator _animator;
	private AudioSource _audio;
    [SerializeField]
	private float _dps = 1;
    [SerializeField]
    private GameObject _weapon;

    private Rigidbody _rigidbody;

    void Start () {
        //_renderer = GetComponentInChildren<Renderer>();
        //_defaultMaterial = _renderer.material;
	    _audio = GetComponent<AudioSource>();
	    _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<DudeMovement> ();
		_movement.OnReachedTarget.AddListener (Melt);
	    _animator = GetComponentInChildren<Animator>();


        FindNewTarget ();
	}

	void Update () {
	    if (_highlighted && Input.GetMouseButton(0) && !DudeMouseGrab.Instance.Grabbed) {
	        DudeMouseGrab.Instance.Grab(this);
	        SetState(DudeState.Grabbed);
            _highlighted = false;
        }

		if (State == DudeState.Melting) {
			_target.AddHealth(-_dps);
		}
		
	}

    void OnMouseEnter() {
        if (DudeMouseGrab.Instance.Grabbed) return;
        //_renderer.material = HightligtMaterial;
        _highlighted = true;
    }
	

    void OnMouseExit() {
        if (DudeMouseGrab.Instance.Grabbed) return;
        //_renderer.material = _defaultMaterial;
        _highlighted = false;
    }

    public void Grab()
    {
        SetState(DudeState.Grabbed);
        _movement.RemovePlanet();
    }
    public void Release(Planet planet)
    {
        //_renderer.material = _defaultMaterial;
        _highlighted = false;
        SetState(DudeState.Falling);
        if (planet) {
            _movement.AlignWithPlanet(planet);
        }
    }

    public void SetState(DudeState state) {
        State = state;
        switch (State)
        {
            case DudeState.Melting:
                _weapon.SetActive(true);
                _animator.SetBool("Walking", false);
                _animator.SetBool("Struggle", false);
                break;
            case DudeState.Walking:
                _weapon.SetActive(false);
                _animator.SetBool("Walking", true);
                _animator.SetBool("Struggle", false);
                break;
            case DudeState.Idle:
                _weapon.SetActive(false);
                _animator.SetBool("Walking", false);
                _animator.SetBool("Struggle", false);
                break;
            case DudeState.Grabbed:
                _weapon.SetActive(false);
                _animator.SetBool("Walking", false);
                _animator.SetBool("Struggle", true);
				_audio.Play ();
                break;
            case DudeState.Falling:
                _weapon.SetActive(false);
                _animator.SetBool("Walking", false);
                _animator.SetBool("Struggle", false);
                break;
            default:
                break;
        }
    }

    public void FindNewTarget() {
        if (State == DudeState.Grabbed) return;
		_target = MeltableBase.GetClosestMeltable (transform.position);
        
        if (_target == null) {
            SetState(DudeState.Idle);
			return;
        } else {
            SetState(DudeState.Walking);
        }

		_movement.SetTarget (_target);

        _target.OnMelted.AddListener (FindNewTarget);
	}

	private void Melt() {
	    SetState(DudeState.Melting);
        _weapon.SetActive(true);
    }

    public void Throw(Vector3 velocity) {
        _rigidbody.velocity = velocity;
        _rigidbody.isKinematic = false;
    }
}
