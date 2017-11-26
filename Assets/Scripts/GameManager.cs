using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private CountScore _scoreText;
  public static bool GameOver { get; private set; }
	[SerializeField]
	private Text _restartText;
	[SerializeField]
	private Text _tipsText;

    public static GameManager Instance { get; private set; }

    void Start(){
	    GameOver = false;

        if (Instance) throw new Exception("Multiple GameManager instances");
        Instance = this;
    }

	public void EndGame(){
		Debug.Log ("Game over!");
	  GameOver = true;
    var planets = new List<Planet>(FindObjectsOfType<Planet>());

		_restartText.gameObject.SetActive (true);
		_tipsText.gameObject.SetActive (false);
		
		foreach (var p in planets)
		{
			StartCoroutine(p.Explode(() => {
				_scoreText.SetTitleText ("Game Over!");
			}));
		}

	}
}
