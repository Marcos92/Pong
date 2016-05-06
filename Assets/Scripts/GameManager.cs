using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Pong pongPrefab;
    public GameObject cornerPrefab;
    public Goal goalPrefab;
	public Ball ballPrefab;
    public Score scoreManager;
    public bool pongs8;
    int pongNumber = 4;
    public float ray;
    public int initialPoints;

    public float matchDurationMinutes;
    public float matchDurationSeconds;
    private float timer;
    private bool gameEnded = false;

    public HUD hud;

    private List<Pong> activePlayers;
    private List<Goal> goals;
    private Ball ball;
    private bool isPaused = false;

	// Use this for initialization
	void Start ()
    {
        
        timer = matchDurationMinutes * 60.0f + matchDurationSeconds;

        Vector3 pongOffset = new Vector3(0, 0, -1) * ray;
        Vector3 cornerOffset = new Vector3(1, 0, -1) * ray;
        activePlayers = new List<Pong>();
        goals = new List<Goal>();

        if (pongs8)
            pongNumber = 8;

        float angle = 360 / pongNumber;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, angle));

        if (pongs8)//quando são oito pongs é preciso ajustar o offset dos cantos
        {
            cornerOffset *= 3f / 4f;
            Quaternion halfRotation = Quaternion.Euler(new Vector3(0, angle / 2));
            cornerOffset = halfRotation * cornerOffset;
        }

        cornerOffset.y = pongOffset.y += 1.5f;
        
        for (int i = 0; i < pongNumber; i++)
        {
            Pong pong = Instantiate(pongPrefab, transform.position + pongOffset, Quaternion.Euler(new Vector3(0, angle * i))) as Pong;
            pong.transform.parent = transform;
            pong.name = "Pong" + i;
            pong.points = initialPoints;
            scoreManager.players.Add(pong);
            activePlayers.Add(pong);

            

            Instantiate(cornerPrefab, transform.position + cornerOffset, Quaternion.identity);
            pong.bounds = Vector3.Distance(pong.transform.position, transform.position + cornerOffset) - pong.transform.GetComponent<Renderer>().bounds.size.x * 0.5f;

            //Atribuir baliza ao pong
            Goal goal = Instantiate(goalPrefab, pong.transform.position - pong.transform.forward * 3.3f, Quaternion.Euler(new Vector3(0, angle * i))) as Goal;
            goal.transform.GetComponent<BoxCollider>().size = new Vector3(pong.bounds * 2.5f, 0, 5f);
            goal.owner = pong;
            goal.gameM = this;
            goal.scoreManager = scoreManager;
            goals.Add(goal);
            //pong.goal = goal;

            if (i == 0)
            {
                pong.controlable = true; //Mudar conforme o jogador
                Pong.myBounds = pong.bounds;
            }

            else
            {
                pong.bot = true;
            }
            
            pongOffset = rotation * pongOffset;
            cornerOffset = rotation * cornerOffset;
		}

        scoreManager.UpdateScores();

        SpawnBall();
    }

    public void SpawnBall()
    {
        ball = Instantiate(ballPrefab, new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity) as Ball;
    }
	
    public void Lost(Pong p)
    {
        p.gameObject.SetActive(false);
        foreach (Goal g in goals)
        {
            if (g.owner == p) g.CloseGoal();
        }
    }

    public void Won(Pong p)
    {
        gameEnded = true;
        ball.gameObject.SetActive(false);
        hud.WinnerScreen(p.name);

    }

    public void NewGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void RemovePlayers()
    {
        List<Pong> toRemove = new List<Pong>();
        if (activePlayers.Count == 1) Won(activePlayers[0]);
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

    void PauseUnpause()
    {
        isPaused = !isPaused;            
        Time.timeScale = 1 - Convert.ToInt32(isPaused);
        hud.PauseUnpause(isPaused);
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

	void Update ()
    {
        if(!gameEnded) timer -= Time.deltaTime;
        hud.timer = timer;
        if (timer <= 0) TimeEnded();
        if (Input.GetKeyDown(KeyCode.P)) PauseUnpause();
        RemovePlayers();
        
	}
}
