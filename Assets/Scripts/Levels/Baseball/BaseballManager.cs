using UnityEngine;
using System.Collections;

public class BaseballManager : GameManager
{
    // Update is called once per frame
    public Batter batter;
    void Update ()
    {
	    if(!gameEnded)
        {
            gameTimer -= Time.deltaTime;
            ballRespawnTimer -= Time.deltaTime;
            if (ballRespawnTimer <= 0)
            {
                SpawnBall();
                ballRespawnTimer = ballRespawnTime;
            }
        }
        hud.timer = gameTimer;
        if (gameTimer <= 0)
            Lost();

        if (Input.GetKeyDown(KeyCode.P))
            PauseUnpause();
        CheckPlayers();
	}

    protected override void OnStart()
    {
        hud.UpdateScoresBaseball(initialPoints, batter.health);
    }

    void CheckPlayers()
    {
        int i = 0;
        foreach(Pong p in activePlayers)
        {
            i += initialPoints - p.points;
        }

        if (initialPoints - i <= 0)
            Lost();
    }

    void Lost()
    {
        foreach (Pong p in activePlayers)
            p.gameObject.SetActive(false);
        foreach (Goal g in goals)
            g.CloseGoal();
        foreach (Ball b in balls)
            Destroy(b.gameObject);
        activePlayers.Clear();
        balls.Clear();
        gameEnded = true;
        hud.LoserScreen();
    }

    public void Won()
    {
        foreach (Ball b in balls)
            Destroy(b.gameObject);
        balls.Clear();
        hud.WinnerScreen("PLAYERS");
        audioSource.clip = activePlayers[0].win;
        audioSource.Play();
        gameEnded = true;
    }
}
