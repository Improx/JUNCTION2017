using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class Planet : MonoBehaviour
{
	[SerializeField] private Explosion _explosion;

	private float _radius;
	public float Radius {
		get {
			if (_radius == 0) {
				_radius = GetComponentInChildren<SphereCollider>().radius * GetComponentInChildren<SphereCollider>().transform.lossyScale.x;
			}
			return _radius;
		}
	}

	public IEnumerator Explode(UnityAction onDone){
		GameObject obj = Instantiate(_explosion.gameObject, transform.position, Quaternion.identity);
		Explosion exp = obj.GetComponent<Explosion>();
		exp.Explode(100);

		gameObject.SetActive(false);

		yield return new WaitForSeconds(exp.Particles.main.duration * 0.5f);

		onDone.Invoke();
	}
}
