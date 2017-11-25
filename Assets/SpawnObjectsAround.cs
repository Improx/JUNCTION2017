using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsAround : MonoBehaviour {

	[SerializeField] private float minSpawnDistance = 10f;
	[SerializeField] private float maxSpawnDistance = 50f;

	[SerializeField] private Transform _prefabToSpawn;
	[SerializeField] private float _spawnRate = 20f;

	private List<ISpaceObject> _spawnedObjects;
	private ParticleSystem _trailSystem;

	private void Start(){
		_trailSystem = GetComponent<ParticleSystem>();
		InvokeRepeating("Spawn", 0f, _spawnRate);
	}

	private void Spawn(){
		var pos = transform.position + Random.onUnitSphere * Random.Range(minSpawnDistance, maxSpawnDistance);
		var obj = Instantiate(_prefabToSpawn);
		obj.SetParent(gameObject.transform, true);
		obj.transform.position = pos;

		ISpaceObject so = obj.GetComponentInChildren<ISpaceObject>();
		print(so);
		print(obj);
		so.StartFlyingTowards(transform);
	}
}
