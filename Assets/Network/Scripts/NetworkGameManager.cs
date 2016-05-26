using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkGameManager : NetworkBehaviour
{
    public GameObject ballPrefab;
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
    internal List<GameObject> balls;
    private bool isPaused = false;

    [HideInInspector]
    public int playerIndex;

    void Awake()
    {
        if (pongs8)
            pongNumber = 8;
    }

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        gameTimer = matchDurationMinutes * 60.0f + matchDurationSeconds;
        ballRespawnTimer = ballRespawnTime;
        balls = new List<GameObject>();

        if (balls.Count < maxBalls) CmdSpawnBall(); //Limita o número de bolas em simultâneo

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    [Command]
    public void CmdSpawnBall()
    {
        GameObject ball = (GameObject)Instantiate(ballPrefab, new Vector3(transform.position.x, 1.5f, transform.position.z), 
            Quaternion.identity);
        ball.GetComponent<Ball>().pongQuartets = pongNumber/4;
        balls.Add(ball);
        NetworkServer.Spawn(ball);
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
        foreach (GameObject b in balls)
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void RemovePlayers()
    {
        //List<Pong> toRemove = new List<Pong>();
        //if (activePlayers.Count == 1)
        //    Won(activePlayers[0]);
        //else
        //{
        //    foreach (Pong p in activePlayers)
        //    {
        //        if (p.points <= 0)
        //        {
        //            Lost(p);
        //            toRemove.Add(p);
        //        }
        //    }
        //    foreach (Pong p in toRemove)
        //    {
        //        activePlayers.Remove(p);
        //    }
        //}
    }

    protected void PauseUnpause()
    {
        isPaused = !isPaused;            
        Time.timeScale = 1 - Convert.ToInt32(isPaused);
        hud.PauseUnpause(isPaused);
        // Switch to bot if paused
    }

    void TimeEnded()
    {
        //Pong bestScore = activePlayers[0];
        //foreach (Pong p in activePlayers)
        //{
        //    if (p.points > bestScore.points) bestScore = p;
        //}
        //Won(bestScore);
    }

	void Update ()
    {
        if (!gameEnded)
        {
            gameTimer -= Time.deltaTime;
            ballRespawnTimer -= Time.deltaTime;
            if (ballRespawnTimer <= 0)
            {
                CmdSpawnBall();
                ballRespawnTimer = ballRespawnTime;
            }
        }
        //hud.timer = gameTimer;
        if (gameTimer <= 0)
            TimeEnded();

        if (Input.GetKeyDown(KeyCode.P))
            PauseUnpause();
        RemovePlayers();
        

	}
}
