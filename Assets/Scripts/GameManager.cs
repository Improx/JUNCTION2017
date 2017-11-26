using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private GameObject _gameOverUI;

	[SerializeField]
	private CountScore _scoreText;

	void Start(){
		Time.timeScale = 1;
	}

	public void EndGame(){
		Debug.Log ("Game over!");

		var planets = new List<Planet>(FindObjectsOfType<Planet>());
		foreach (var p in planets)
		{
			StartCoroutine(p.Explode(() => {
				_gameOverUI.SetActive (true);
				_scoreText.SetTitleText ("Game Over!");
				Time.timeScale = 0;
			}));
		}

	}
}
