using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScore : MonoBehaviour {

	[SerializeField]
	private List<Text> _scoreTexts;

	private int _score;
	[SerializeField]
	private int _pointsPerSecond = 1;

	// Use this for initialization
	void Start () {
		_score = 0;
		SetScoreTexts ();
		InvokeRepeating ("AddPoints", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void AddPoints () {
		_score += _pointsPerSecond;
		SetScoreTexts ();
	}

	void SetScoreTexts(){
		foreach (var text in _scoreTexts) {
			text.text = "Points: " + _score.ToString();
		}
	}
}
