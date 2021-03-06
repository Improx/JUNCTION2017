﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : Grabbable, ISpaceObject {

	[SerializeField] private float minTorque = 10f;
	[SerializeField] private float maxTorque = 30f;
	[SerializeField] private float _minFlyForce = 100f;
	[SerializeField] private float _maxFlyForce = 300f;

	[SerializeField] private Explosion _hitExplosion;

	private Rigidbody _rb;

	private void Awake(){
		_rb = GetComponentInChildren<Rigidbody>();
		
	}

	public void StartFlyingTowards(Transform target){
		Vector3 vecToPlanet = (target.position - transform.position).normalized;
		Debug.DrawRay(transform.position, vecToPlanet * 10000f, Color.red, 10000f);
		_rb.AddForce(vecToPlanet * Random.Range(_minFlyForce, _maxFlyForce));

		var randomTorque = Random.Range(minTorque, maxTorque);
		var torque = new Vector3(randomTorque, randomTorque, randomTorque);
		_rb.AddTorque(torque);
	}

	private void OnCollisionEnter(Collision collision){
		var hit = collision.collider.transform.root.GetComponentInChildren<Planet>();

		if(hit == null && collision.rigidbody) {
		    var grabbable = collision.rigidbody.GetComponent<Grabbable>();


		    if (grabbable)
		    {
		        if (grabbable.Flying) CountScore.Instance.AddPoints(100);
                Destroy(grabbable.gameObject);
		    }
        }

	    if (hit) {
	        foreach (var icecap in hit.GetComponentsInChildren<Icecap>()) {
	            icecap.AddHealth(-200);
            }
	    }

		Destroy(gameObject);

		GameObject exp = Instantiate(_hitExplosion.gameObject, transform.position, Quaternion.identity);
		exp.GetComponent<Explosion>().Explode(60);
	}

    public override void Throw(Vector3 velocity)
    {
        _rb.velocity = velocity;
        _rb.isKinematic = false;

        var randomTorque = Random.Range(10f, 30f);
        var torque = new Vector3(randomTorque, randomTorque, randomTorque);
        _rb.AddTorque(torque);
    }

    public override void Grab() {
        _rb.useGravity = false;
        _rb.isKinematic = true;

        transform.localPosition = new Vector3(0f, 0.030f, 0f);
    }
}
