using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScore : MonoBehaviour {

	[SerializeField]
	private Text _titleText;
	[SerializeField]
	private Text _scoreText;

	private int _score;
	[SerializeField]
	private int _pointsPerSecond = 1;

    public static CountScore Instance { get; private set; }
	// Use this for initialization
	void Start () {
		_score = 0;
		SetScoreText ();
		InvokeRepeating ("AddPoints", 1.0f, 1.0f);

	    if (Instance) throw new Exception("Multiple CountScore instances");
	    Instance = this;

    }

    // Update is called once per frame
    void AddPoints () {
		_score += _pointsPerSecond;
		SetScoreText ();
    }
    public void AddPoints(int points)
    {
        _score += points;
        SetScoreText();
    }

    private void SetScoreText (){
		_scoreText.text = _score.ToString();
	}

	public void SetTitleText(string text){
		_titleText.text = text;
    }
}
