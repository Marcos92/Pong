using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager2 : GameManager
{

    int pongNumber = 4;
    float bounds;
    public float radius;
    public float extraMargin;//graus adicionais ao min e max para as palets nao entrarem dentro dos cilindros
    
    private float gameTimer, ballRespawnTimer;
    private bool gameEnded = false;

    private List<Pong> activePlayers;
    private List<Goal> goals;
    internal List<Ball> balls = new List<Ball>();
    private bool isPaused = false;

    public bool pongLookigAtCenter;

	// Use this for initialization
	void Start ()
    {
        CircularStart();
    }

    void CircularStart()
    {
        gameTimer = matchDurationMinutes * 60.0f + matchDurationSeconds;
        ballRespawnTimer = ballRespawnTime;

        activePlayers = new List<Pong>();
        goals = new List<Goal>();

        if (pongs8)
            pongNumber = 8;

        //Para colocar o primerio centrado no eixo dos Z
        float degreePerPerson = 360.0f / (pongNumber -1);
        float initialDegrees = 270.0f;

        for (int i = 0; i < pongNumber - 1; i++)
        {

            //Estou a por longe para nao coliderem com a bola instantaneamente. eles vai ao sitio por si no log primeiro update deles
            PongCircularMove pong = Instantiate(pongPrefab, new Vector3(200,0.0f,200), Quaternion.identity) as PongCircularMove;
            pong.transform.parent = transform;
            pong.name = "Pong" + i;
            pong.points = initialPoints;

            pong.minDegrees = initialDegrees - degreePerPerson / 2 + degreePerPerson * i + extraMargin;
            pong.maxDegrees = initialDegrees + degreePerPerson / 2 + degreePerPerson * i - extraMargin;
            pong.distanceToCenter = radius;
            pong.currentAngle = (pong.minDegrees + pong.maxDegrees) / 2;
            pong.lookAtCenter = pongLookigAtCenter;

            hud.players.Add(pong);
            activePlayers.Add(pong);

            float degrees = (initialDegrees + degreePerPerson / 2 + degreePerPerson * i )* Mathf.PI / 180;
            float x = Mathf.Cos(degrees) * radius;
            float z = Mathf.Sin(degrees) * radius;

            Instantiate(cornerPrefab, new Vector3(x, transform.position.y, z), Quaternion.identity);

            ////Atribuir baliza ao pong
            //Goal goal = Instantiate(goalPrefab, pong.transform.position - pong.transform.forward * 3.3f, Quaternion.Euler(new Vector3(0, angle * i))) as Goal;
            //goal.transform.GetComponent<BoxCollider>().size = new Vector3(bounds * (pongs8 ? 2.7f : 2.2f), 0, 5f);
            //goal.owner = pong;
            //goal.gameM = this;
            //goal.hud = hud;
            //goals.Add(goal);

            if (i == 0)
            {
                pong.controlable = true; //Mudar conforme o jogador
            }

            else
            {
                pong.bot = true;
            }

        }

        hud.UpdateScores();

        if (balls.Count < maxBalls) SpawnBall(); //Limita o número de bolas em simultâneo
    }

    public void SpawnBall()
    {
        Ball ball = Instantiate(ballPrefab, new Vector3(transform.position.x, 0.0f, transform.position.z),
            Quaternion.identity) as Ball;
        ball.pongQuartets = pongNumber / 4;
        balls.Add(ball);
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
        foreach (Ball b in balls)
        {
            Destroy(b.gameObject);
        }
        balls.Clear();
        hud.WinnerScreen(p.name);
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

    void Update()
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
