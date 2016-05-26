using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManagerMata : GameManager
{
    private int currentMid = 0;
    public float switchInterval = 20.0f;
    private float timer = 0.0f;
    private Vector3 centerPosition = new Vector3(0.0f, 1.5f, 0.0f);
    public List<Team> teams;
    protected override void OnStart()
    {
        teams = new List<Team>();
        for (int i = 0; i < pongNumber / 2; i++)
        {
            Team t = new Team();
            t.members = new List<Pong>();
            t.score = initialPoints;
            t.number = i + 1;
            teams.Add(t);
        }

        int n = 0;
        foreach (Pong p in activePlayers)
        {
            p.oldPos = p.transform.position;
            p.oldRotation = p.transform.rotation;

            p.midXMin = -ray / 3;
            p.midXMax = ray / 3;
            p.midZmin = -ray / 3;
            p.midZMax = ray / 3;
            p.team = n / 2;
            teams[n / 2].members.Add(p);
            n++;
        }
        hud.UpdateScoresTeams(teams);
        Invoke("PlaceFirstMid", 0.1f);
    }

    private void PlaceFirstMid()
    {
        activePlayers[0].isMid = true;
        activePlayers[0].transform.position = centerPosition;
        foreach (Goal g in goals)
        {
            if (g.owner == activePlayers[currentMid])
            {
                g.CloseGoal();
            }
        }
    }

    public void ScoreGoal(int team, bool ownTeam)
    {
        if(ownTeam) teams[team].score++;
        else teams[team].score--;
        hud.UpdateScoresTeams(teams);
    }

    private void SwitchMid()
    {
        int newMid = currentMid;
        if (currentMid + 1 > activePlayers.Count - 1) newMid = 0;
        else newMid++;

        activePlayers[newMid].isMid = true;
        activePlayers[newMid].transform.position = centerPosition;
        activePlayers[newMid].transform.rotation = Quaternion.identity;

        activePlayers[currentMid].isMid = false;

        activePlayers[currentMid].transform.position = activePlayers[currentMid].oldPos;
        activePlayers[currentMid].transform.localPosition = activePlayers[currentMid].oldPos;
        activePlayers[currentMid].transform.rotation = activePlayers[currentMid].oldRotation;

        foreach (Goal g in goals)
        {
            if (g.owner == activePlayers[newMid])
            {
                g.CloseGoal();
            }
            if (g.owner == activePlayers[currentMid])
            {
                g.ReOpen();
            }
        }
        currentMid = newMid;


    }

    public override void SpawnBall()
    {
        int corn = UnityEngine.Random.Range(0, corners.Count);
        Vector3 spawnPos = corners[corn].transform.position - corners[corn].transform.position.normalized;
        Ball ball = Instantiate(ballPrefab, spawnPos,
            Quaternion.identity) as Ball;
        ball.cornerSpawn = true;
        ball.gManager = this;
        balls.Add(ball);
    }

    void RemovePlayers()
    {
        List<Pong> toRemove = new List<Pong>();
        if (activePlayers.Count == 1)
            Won(activePlayers[0]);
        else
        {
            foreach (Pong p in activePlayers)
            {
                if (p.points <= 0)
                {
                    Lost(p);
                    toRemove.Add(p);
                }
            }
            foreach (Pong p in toRemove)
            {
                activePlayers.Remove(p);
            }
        }
    }

    void TimeEnded()
    {
        Pong bestScore = activePlayers[0];
        foreach (Pong p in activePlayers)
        {
            if (p.points > bestScore.points) bestScore = p;
        }
        Won(bestScore);
    }

    void Update()
    {
        if (!gameEnded)
        {
            timer += Time.deltaTime;
            gameTimer -= Time.deltaTime;
            ballRespawnTimer -= Time.deltaTime;

            if (timer >= switchInterval)
            {
                SwitchMid();
                timer = 0.0f;
            }

            if (ballRespawnTimer <= 0)
            {
                SpawnBall();
                ballRespawnTimer = ballRespawnTime;
            }
        }
        hud.timer = gameTimer;
        if (gameTimer <= 0)
            TimeEnded();

        if (Input.GetKeyDown(KeyCode.P))
            PauseUnpause();
        RemovePlayers();


    }
}
