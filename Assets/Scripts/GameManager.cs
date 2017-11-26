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
		_scoreText.SetTitleText ("Game Over!");
		Time.timeScale = 0;
	}
}
