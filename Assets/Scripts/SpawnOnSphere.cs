using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnSphere : MonoBehaviour {

	[SerializeField] private Dude _prefabToSpawn;
	[SerializeField] private float _spawnDelay = 10f;
	[SerializeField] private float _spawnDecayAmount = 0.05f;
	[SerializeField] private float _spawnRadius = 2f;
	private float _nextSpawnTime;

	private List<Dude> _spawnedDudes = new List<Dude>();

	private Planet _planet;

	private void Start() {
	    _planet = GetComponentInParent<Planet>();
	}

	private void Update(){
		CheckSpawnTimeAndSpawn();
	}

	private void CheckSpawnTimeAndSpawn(){
		if(Time.time >= _nextSpawnTime){
			Spawn();
			_spawnDelay -= _spawnDecayAmount;
		    _spawnDelay = Mathf.Max(_spawnDelay, 0.05f);
            _nextSpawnTime = Time.time + _spawnDelay;
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
