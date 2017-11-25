using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private GameObject _gameOverUI;

	void Start(){
		Time.timeScale = 1;
	}

	public void EndGame(){
		Debug.Log ("Game over!");
		_gameOverUI.SetActive (true);
		Time.timeScale = 0;
	}
}
