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
    public float ballRespawnTime;
    public int maxBalls = 10;

    public bool pongs8;
    internal int pongNumber = 4;
    float bounds;
    public float ray, boundsDecrement;

    public int initialPoints;

    public float matchDurationMinutes;
    public float matchDurationSeconds;
    
    protected float gameTimer, ballRespawnTimer;
    protected bool gameEnded = false;

    public HUD hud;
    protected AudioSource audioSource;

    protected List<Pong> activePlayers;
    protected List<Goal> goals;
    internal List<Ball> balls;
    private bool isPaused = false;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        gameTimer = matchDurationMinutes * 60.0f + matchDurationSeconds;
        ballRespawnTimer = ballRespawnTime;

        Vector3 pongOffset = new Vector3(0, 0, -1) * ray;
        Vector3 cornerOffset = new Vector3(1, 0, -1) * ray;
        activePlayers = new List<Pong>();
        goals = new List<Goal>();
        balls = new List<Ball>();

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
        bounds = Vector3.Distance(transform.position + pongOffset, transform.position + cornerOffset) - 
            pongPrefab.transform.GetComponent<BoxCollider>().bounds.size.x * 0.5f - boundsDecrement;

        for (int i = 0; i < pongNumber; i++)
        {
            Pong pong = Instantiate(pongPrefab, transform.position + pongOffset, Quaternion.Euler(new Vector3(0, angle * i))) as Pong;
            pong.transform.parent = transform;
            pong.name = "Pong" + i;
            pong.points = initialPoints;
            pong.bounds = bounds;
            hud.players.Add(pong);
            activePlayers.Add(pong);            

            Instantiate(cornerPrefab, transform.position + cornerOffset, Quaternion.identity);
            
            //Atribuir baliza ao pong
            Goal goal = Instantiate(goalPrefab, pong.transform.position - pong.transform.forward * 5f, Quaternion.Euler(new Vector3(0, angle * i))) as Goal;
            goal.transform.GetComponent<BoxCollider>().size = new Vector3(bounds * (pongs8? 2.7f : 2.7f), 0, 5f);
            goal.owner = pong;
            goal.gameM = this;
            goal.hud = hud;
            goals.Add(goal);

            if (i == 0)
            {
                pong.controlable = true; //Mudar conforme o jogador
            }

            else
            {
                pong.bot = true;
            }
            
            pongOffset = rotation * pongOffset;
            cornerOffset = rotation * cornerOffset;
		}

        hud.UpdateScores();

        if(balls.Count < maxBalls) SpawnBall(); //Limita o número de bolas em simultâneo

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    public void SpawnBall()
    {
        Ball ball = Instantiate(ballPrefab, new Vector3(transform.position.x, 1.5f, transform.position.z), 
            Quaternion.identity) as Ball;
        ball.pongQuartets = pongNumber/4;
        balls.Add(ball);
    }
	
    public void Lost(Pong p)
    {
        p.gameObject.SetActive(false);
        foreach (Goal g in goals)
        {
            if (g.owner == p)
            {
                g.CloseGoal();
                //audioSource.clip = p.lose;
                //audioSource.Play();
            }
        }
    }

    public void Won(Pong p)
    {
        gameEnded = true;
        foreach (Ball b in balls)
        {
            Destroy(b.gameObject);
        }
        balls.Clear();
        hud.WinnerScreen(p.name);
        audioSource.clip = p.win;
        audioSource.Play();
    }

    public void NewGame()
    {
        Application.LoadLevel(Application.loadedLevel);
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

    protected void PauseUnpause()
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
        if (!gameEnded)
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
            TimeEnded();

        if (Input.GetKeyDown(KeyCode.P))
            PauseUnpause();
        RemovePlayers();
        

	}
}
