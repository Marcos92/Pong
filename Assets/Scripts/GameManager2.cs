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

    private List<PongCircularMove> activePlayers;
    private List<Goal> goals;
    internal List<Ball> balls = new List<Ball>();
    private bool isPaused = false;
    public float switchInterval;
    private float switchTimer = 0;

    public bool pongLookigAtCenter;
    public float centerMoveRadius;

    //public PongMid centerPongPrefab;

    public List<Team> equipas;

    // Use this for initialization
    void Start()
    {
        CircularStart();
    }

    void CircularStart()
    {
        equipas = new List<Team>();

        gameTimer = matchDurationMinutes * 60.0f + matchDurationSeconds;
        ballRespawnTimer = ballRespawnTime;

        activePlayers = new List<PongCircularMove>();
        goals = new List<Goal>();

        if (pongs8)
            pongNumber = 8;

        for (int i = 0; i < pongNumber / 2; i++)
        {
            Team t = new Team();
            t.members = new List<PongCircularMove>();
            t.score = initialPoints;
            t.number = i + 1;
            equipas.Add(t);
            //hud.equipas.Add(t);
        }

        //Para colocar o primerio centrado no eixo dos Z
        float degreePerPerson = 360.0f / (pongNumber - 1);
        float initialDegrees = 270.0f;

        for (int i = 0; i < pongNumber - 1; i++)
        {

            //Estou a por longe para nao coliderem com a bola instantaneamente. eles vai ao sitio por si no log primeiro update deles
            PongCircularMove pong = Instantiate(pongPrefab, new Vector3(200, 0.0f, 200), Quaternion.identity) as PongCircularMove;
            pong.transform.parent = transform;
            pong.name = "Pong" + i;
            pong.points = initialPoints;

            pong.minDegrees = initialDegrees - degreePerPerson / 2 + degreePerPerson * i + extraMargin;
            pong.maxDegrees = initialDegrees + degreePerPerson / 2 + degreePerPerson * i - extraMargin;
            pong.distanceToCenter = radius;
            pong.currentAngle = (pong.minDegrees + pong.maxDegrees) / 2;
            pong.lookAtCenter = pongLookigAtCenter;
            pong.isMid = false;
            pong.team = i / 2;
            if (pong.team == 0) pong.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
            else pong.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
            equipas[i / 2].members.Add(pong);

            hud.players.Add(pong);
            activePlayers.Add(pong);

            float degrees = (initialDegrees + degreePerPerson / 2 + degreePerPerson * i) * Mathf.PI / 180;
            float x = Mathf.Cos(degrees) * radius;
            float z = Mathf.Sin(degrees) * radius;

            Instantiate(cornerPrefab, new Vector3(x, transform.position.y, z), Quaternion.identity);

            //Atribuir baliza ao pong
            //Goal goal = Instantiate(goalPrefab, pong.transform.position - pong.transform.forward * 3.3f, Quaternion.Euler(new Vector3(0, angle * i))) as Goal;
            //goal.transform.GetComponent<BoxCollider>().size = new Vector3(bounds * (pongs8 ? 2.7f : 2.2f), 0, 5f);
            //goal.owner = pong;
            //goal.gameM = this;
            //goal.hud = hud;
            //goals.Add(goal);

            if (i == 0)
            {
                pong.controlable = true; //Mudar conforme o jogador
                pong.bot = false;
            }

            else
            {
                pong.controlable = false;
                pong.bot = true;
                pong.RamdomDistanceToDelay = 0.2f;
            }

        }

        //Place at center
        //PongMid pongmid = Instantiate(centerPongPrefab, Vector3.zero, Quaternion.identity) as PongMid;
        PongCircularMove pongmid = Instantiate(pongPrefab, Vector3.zero, Quaternion.identity) as PongCircularMove;
        pongmid.transform.parent = transform;
        pongmid.name = "Pong" + pongNumber;
        pongmid.points = initialPoints;
        pongmid.moveRadius = centerMoveRadius;
        pongmid.team = (pongNumber - 1) / 2;
        pongmid.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
        pongmid.isMid = true;
        pongmid.controlable = false;
        pongmid.bot = true;
        pongmid.speed = 20;
        activePlayers.Add(pongmid);

        //hud.UpdateScores();
        hud.UpdateScoresTeams(equipas);

        if (balls.Count < maxBalls) SpawnBall(); //Limita o número de bolas em simultâneo
    }

    public void SpawnBall()
    {
        BallMata ball = Instantiate(ballPrefab, new Vector3(transform.position.x, 0.0f, transform.position.z),
            Quaternion.identity) as BallMata;
        ball.pongQuartets = pongNumber / 4;
        ball.gManager = this;
        balls.Add(ball);
    }

    public void SwitchMiddle()
    {
        PongCircularMove pTemp = new PongCircularMove();
        bool wasBot;
        float oldSpeed = 0;
        int oldmid = 0;
        foreach (PongCircularMove p in activePlayers)
        {
            if (p.isMid)
            {
                pTemp = p;
                oldmid = activePlayers.IndexOf(p);
            }
        }

        int newMid = oldmid + 1;
        if (newMid >= pongNumber - 1) newMid = 0;

        activePlayers[oldmid].transform.position = activePlayers[newMid].transform.position;
        activePlayers[oldmid].currentAngle = activePlayers[newMid].currentAngle;
        activePlayers[oldmid].minDegrees = activePlayers[newMid].minDegrees;
        activePlayers[oldmid].maxDegrees = activePlayers[newMid].maxDegrees;
        oldSpeed = activePlayers[oldmid].speed;
        activePlayers[oldmid].speed = activePlayers[newMid].speed;
        activePlayers[oldmid].isMid = false;
        activePlayers[oldmid].distanceToCenter = radius;
        activePlayers[oldmid].lookAtCenter = true;
        //wasBot = activePlayers[oldmid].bot;
        //activePlayers[oldmid].bot = activePlayers[newMid].bot;
        //activePlayers[oldmid].controlable = activePlayers[newMid].controlable;

        activePlayers[newMid].transform.position = Vector3.zero;
        activePlayers[newMid].isMid = true;
        activePlayers[newMid].transform.rotation = Quaternion.identity;
        activePlayers[newMid].speed = oldSpeed;
        activePlayers[newMid].distanceToCenter = pTemp.distanceToCenter;
        activePlayers[newMid].lookAtCenter = false;
        //activePlayers[newMid].bot = wasBot;
        //activePlayers[newMid].controlable = !wasBot;

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
        List<PongCircularMove> toRemove = new List<PongCircularMove>();
        if (activePlayers.Count == 1)
            Won(activePlayers[0]);
        else
        {
            foreach (PongCircularMove p in activePlayers)
            {
                if (p.points <= 0)
                {
                    Lost(p);
                    toRemove.Add(p);
                }
            }
            foreach (PongCircularMove p in toRemove)
            {
                activePlayers.Remove(p);
            }
        }
    }

    public void UpdateScores()
    {
        hud.UpdateScoresTeams(equipas);
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
            switchTimer += Time.deltaTime;
            if (switchTimer >= switchInterval)
            {
                SwitchMiddle();
                switchTimer = 0.0f;
            }
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

[Serializable]
public class Team
{
    public List<PongCircularMove> members;
    public int score;
    public int number;
}
