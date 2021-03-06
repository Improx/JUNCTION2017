﻿using System;
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
    public static bool GameOver { get; private set; }
    public static bool GameStart { get; private set; }

    public static GameManager Instance { get; private set; }

    void Start(){
	    GameOver = false;
        GameStart = false;

        if (Instance) throw new Exception("Multiple GameManager instances");
        Instance = this;
        _tipsText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(false);
    }

	public void EndGame(){
		Debug.Log ("Game over!");
	    GameOver = true;
        var planets = new List<Planet>(FindObjectsOfType<Planet>());

		_restartText.gameObject.SetActive (true);
		
		foreach (var p in planets)
		{
			StartCoroutine(p.Explode(() => {
				_scoreText.SetTitleText ("Game Over!");
			}));
		}

	}

    public static void StartGame() {
        GameStart = true;
        Instance._tipsText.gameObject.SetActive(false);
    }
}
