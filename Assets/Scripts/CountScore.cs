using System;
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

    public static CountScore Instance { get; private set; }
	// Use this for initialization
	void Start () {
		_score = 0;
		SetScoreTexts ();
		InvokeRepeating ("AddPoints", 1.0f, 1.0f);

	    if (Instance) throw new Exception("Multiple CountScore instances");
	    Instance = this;

    }

    // Update is called once per frame
    public void AddPoints() {
        _score += _pointsPerSecond;
        SetScoreTexts();
    }
    public void AddPoints(int points)
    {
        _score += points;
        SetScoreTexts();
    }

    void SetScoreTexts(){
		foreach (var text in _scoreTexts) {
			text.text = "Points: " + _score.ToString();
		}
	}
}
