using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Pong owner;
    public GameManager gameM;
    public HUD hud;
    AudioSource aSource;
    public AudioClip goalClip;
    private bool isActive = true;

	void Start () {
	    aSource = GetComponent<AudioSource>();
	}
	

	void Update () {
	    
	}

    void ScoreGoal()
    {
        owner.points -= 1;
        if (gameM is BaseballManager)
            hud.UpdateScoresBaseball(gameM.initialPoints, (gameM as BaseballManager).batter.health);
        else if(gameM is GameManagerMata)
        {
            GameManagerMata g = gameM as GameManagerMata;
            g.ScoreGoal(owner.team,false);
        }
        else
            hud.UpdateScores();
        aSource.clip = goalClip;
        aSource.Play();
    }

    public void CloseGoal()
    {
        isActive = false;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(this.gameObject.transform);
        cube.transform.rotation = transform.rotation;
        cube.transform.position = transform.position;
        cube.transform.localScale = GetComponent<BoxCollider>().size;
    }

    public void ReOpen()
    {
        isActive = true;
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            Ball b = other.GetComponent<Ball>();
            if (b)
            {
                ScoreGoal();
                gameM.balls.Remove(b);
                Destroy(other.gameObject);
                if(gameM.balls.Count == 0) gameM.SpawnBall(); //Só faz spawn de outra bola caso não exista nenhuma de modo a evitar existirem demasiadas bolas em jogo.
            }
        }
    }
}
