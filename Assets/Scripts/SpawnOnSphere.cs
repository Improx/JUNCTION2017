﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnSphere : MonoBehaviour {

	[SerializeField] private Dude _prefabToSpawn;
	[SerializeField] private float _spawnDelay = 10f;
	[SerializeField] private float _spawnDecayAmount = 0.05f;
	private float _nextSpawnTime;

	private List<Dude> _spawnedDudes = new List<Dude>();

	private Planet _planet;

	private void Start(){
		_planet = FindObjectOfType<Planet>();
	}

	private void Update(){
		CheckSpawnTimeAndSpawn();
	}

	private void CheckSpawnTimeAndSpawn(){
		if(Time.time >= _nextSpawnTime){
			Spawn();
			_spawnDelay -= _spawnDecayAmount;
			_nextSpawnTime = Time.time + _spawnDelay;
		}
	}

	private void Spawn(){
		var dude = Instantiate(_prefabToSpawn);
		dude.transform.position = transform.position;
		dude.transform.SetParent(_planet.transform, true);
		dude.transform.localScale = Vector3.one * 0.2f;
		_spawnedDudes.Add(dude);
	}
}