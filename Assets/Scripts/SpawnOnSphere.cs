using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnSphere : MonoBehaviour {

	[SerializeField] private Dude _prefabToSpawn;
	[SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _startTime = 8f;

    private float _nextSpawnTime;
	private int _dudesSpawned = 0;

	private List<Dude> _spawnedDudes = new List<Dude>();

	private Planet _planet;
	float _spawnDelay = 0f;

	private void Start() {
	    _planet = GetComponentInParent<Planet>();
	}

	private void Update() {
	    if (!GameManager.GameStart || GameManager.GameOver) return;
		CheckSpawnTimeAndSpawn();
	}

	private void CheckSpawnTimeAndSpawn(){
		if(Time.time >= _nextSpawnTime){
			Spawn();
			_spawnDelay = _startTime * Mathf.Exp(-(1f/10f)*_dudesSpawned) + 0.35f;
		    _spawnDelay = Mathf.Max(_spawnDelay, 0.35f);
            print(_spawnDelay);
            _nextSpawnTime = Time.time + _spawnDelay;
			_dudesSpawned++;
		}
	}

	private void Spawn(){
		var dude = Instantiate(_prefabToSpawn);

		Vector2 offset = Random.insideUnitCircle;
		dude.transform.position = transform.position + new Vector3(offset.x, 0f, offset.y) * _spawnRadius;
		dude.transform.SetParent(_planet.transform, true);
		dude.transform.localScale = Vector3.one * 0.2f;
		_spawnedDudes.Add(dude);
        dude.GetComponent<DudeMovement>().AlignWithPlanet(_planet);
	    dude.GetComponent<DudeMovement>().SnapToSurface();
	}
}
