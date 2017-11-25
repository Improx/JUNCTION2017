using UnityEngine;

public class Planet : MonoBehaviour {

	private float _radius;
	public float Radius {
		get {
			if (_radius == 0) {
				_radius = GetComponent<MeshFilter> ().mesh.bounds.size.x;
			}
			print (_radius);
			return _radius;
		}
	}

	void Start(){
	}
}
