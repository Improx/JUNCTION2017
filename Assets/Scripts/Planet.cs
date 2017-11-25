using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject Highlight;

	private float _radius;
	public float Radius {
		get {
			if (_radius == 0) {
				_radius = GetComponentInChildren<SphereCollider>().radius * GetComponentInChildren<SphereCollider>().transform.lossyScale.x;
			}
			return _radius;
		}
	}

	void Update() {
	   Highlight.SetActive(false);
    }
}
