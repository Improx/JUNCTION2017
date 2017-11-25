using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsAround : MonoBehaviour {

	[SerializeField] private float minSpawnDistance = 10f;
	[SerializeField] private float maxSpawnDistance = 50f;

	[SerializeField] private Transform _prefabToSpawn;

	private List<ISpaceObject> _spawnedObjects;

	private void Start(){
		Spawn();
	}

	private void Spawn(){
		var pos = transform.position + Random.onUnitSphere * Random.Range(minSpawnDistance, maxSpawnDistance);
		var obj = Instantiate(_prefabToSpawn);
		obj.SetParent(gameObject.transform, true);
		obj.transform.position = pos;

		ISpaceObject so = obj.GetComponent<ISpaceObject>();
		print(so);
		print(obj);
		so.StartFlyingTowards(transform);
	}
}
