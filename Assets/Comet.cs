using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour, ISpaceObject {

	[SerializeField] private float minTorque = 10f;
	[SerializeField] private float maxTorque = 30f;
	[SerializeField] private float _minFlyForce = 100f;
	[SerializeField] private float _maxFlyForce = 300f;

	public void StartFlyingTowards(Transform target){
		Vector3 vecToPlanet = (target.position - transform.position).normalized;
		Debug.DrawRay(transform.position, vecToPlanet * 10000f, Color.red, 10000f);
		GetComponent<Rigidbody>().AddForce(vecToPlanet * Random.Range(_minFlyForce, _maxFlyForce));

		float randomTorque = Random.Range(minTorque, maxTorque);
		Vector3 torque = new Vector3(randomTorque, randomTorque, randomTorque);
		GetComponent<Rigidbody>().AddTorque(torque);
	}

	private void OnCollisionEnter(Collision collision){
		Planet hit = collision.collider.transform.root.GetComponentInChildren<Planet>();

		if(hit == null){
			print("hit object wasn't a planet");
			return;
		}

		Destroy(gameObject);
	}
}
