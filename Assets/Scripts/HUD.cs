﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    //Score
    public List<Pong> players;
    public Text scoreText;
    public bool playerFound = false;

    //Timer
    public Text CounterText;
    private float segundos, minutos;
    public float timer;

    //Pause
    public Text pausedText;

    //Winner
    public Text winnerText;

    //New Game
    public Button restartButton;

	void Start () {
	
	}

    public void WinnerScreen(string playerName)
    {
        winnerText.text = playerName + " WON";
        winnerText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void PauseUnpause(bool isPaused)
    {
        pausedText.gameObject.SetActive(isPaused);
    }

    public void UpdateScores()
    {
        string score = "Scores:\t\t";
        foreach (Pong p in players)
        {
            score += p.name + ": ";
            if (p.points == 0) score += "Lost";
            else score += p.points;
            score += "\t\t";
        }
        scoreText.text = score;
    }

    void CountDown()
    {
        minutos = (int)(timer / 60f);
        segundos = (int)(timer % 60f);
        CounterText.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

	void Update () {
        /*minutos = (int)(Time.time / 60f);
        segundos = (int)(Time.time % 60f);
        CounterText.text = minutos.ToString("00") + ":" + segundos.ToString("00");*/
        CountDown();
	}
}
