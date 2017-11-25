using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScore : MonoBehaviour {

	public Text _scoreText;

	private int _score;
	[SerializeField]
	private int _pointsPerSecond = 1;

	// Use this for initialization
	void Start () {
		_score = 0;
		SetScoreText ();
		InvokeRepeating ("AddPoints", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void AddPoints () {
		_score += _pointsPerSecond;
		SetScoreText ();
	}

	void SetScoreText(){
		_scoreText.text = "Points: " + _score.ToString();
	}
}
