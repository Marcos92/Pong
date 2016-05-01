using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Score : MonoBehaviour {
    public List<Pong> players;
    public Text scoreText;
    public bool playerFound = false;

	// Use this for initialization
	void Start () {
	}

    public void UpdateScores()
    {
        string score = "Scores:\t\t";
        foreach (Pong p in players)
        {
            score += p.name + ": " + p.points + "\t\t";
        }
        scoreText.text = score;
    }

	// Update is called once per frame
	void Update () {
        
	}
}
