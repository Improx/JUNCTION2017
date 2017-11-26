using UnityEngine;

public class Dude : Grabbable
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

	[SerializeField] private GameObject _dudeModel;

	[SerializeField] private ColorRange skinColorRange;
	[SerializeField] private ColorRange shirtColorRange;
	[SerializeField] private ColorRange pantsColorRange;

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

		RandomizeColors ();

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

    public override void Grab()
    {
        SetState(DudeState.Grabbed);
        _movement.RemovePlanet();
        _rigidbody.isKinematic = true;
    }
    /*public override void Release(Planet planet)
    {
        //_renderer.material = _defaultMaterial;
        _highlighted = false;
        SetState(DudeState.Falling);
        if (planet) {
            _movement.AlignWithPlanet(planet);
        }
    }*/

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
                DoYell();
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

    private void DoYell() {
        _audio.pitch = Random.Range(0.85f, 0.1f);
		_audio.Play ();
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

    public override void Throw(Vector3 velocity) {
        _rigidbody.velocity = velocity;
        _rigidbody.isKinematic = false;

        var randomTorque = Random.Range(10f, 30f);
        var torque = new Vector3(randomTorque, randomTorque, randomTorque);
        _rigidbody.AddTorque(torque);
    }

	private void RandomizeColors(){
		var skinColor = Random.ColorHSV (skinColorRange.hueMin, skinColorRange.hueMax, skinColorRange.satMin, skinColorRange.satMax, skinColorRange.valMin, skinColorRange.valMax);
		var shirtColor = Random.ColorHSV (shirtColorRange.hueMin, shirtColorRange.hueMax, shirtColorRange.satMin, shirtColorRange.satMax, shirtColorRange.valMin, shirtColorRange.valMax);
		var pantsColor = Random.ColorHSV (pantsColorRange.hueMin, pantsColorRange.hueMax, pantsColorRange.satMin, pantsColorRange.satMax, pantsColorRange.valMin, pantsColorRange.valMax);

		_dudeModel.GetComponent<Renderer> ().materials [0].color = shirtColor;
		_dudeModel.GetComponent<Renderer> ().materials [1].color = pantsColor;
		_dudeModel.GetComponent<Renderer> ().materials [2].color = skinColor;
	}
}

[System.Serializable]
public class ColorRange{
	public float hueMin;
	public float hueMax;
	public float satMin;
	public float satMax;
	public float valMin;
	public float valMax;
}