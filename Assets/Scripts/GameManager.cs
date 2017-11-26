using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private CountScore _scoreText;
	[SerializeField]
	private Text _restartText;
	[SerializeField]
	private Text _tipsText;

	void Start(){
		Time.timeScale = 1;
	}

	public void EndGame(){
		Debug.Log ("Game over!");

		_restartText.gameObject.SetActive (true);
		_tipsText.gameObject.SetActive (false);

		var planets = new List<Planet>(FindObjectsOfType<Planet>());
		foreach (var p in planets)
		{
			StartCoroutine(p.Explode(() => {
				_scoreText.SetTitleText ("Game Over!");
				Time.timeScale = 0;
			}));
		}

	}
}
