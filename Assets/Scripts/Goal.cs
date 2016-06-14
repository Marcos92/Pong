using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Pong owner;
    public GameManager gameM;
    public HUD hud;
    AudioSource aSource;
    public AudioClip goalClip;
    private bool isActive = true;
    public GameObject boulder;

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

        if(gameM is GameManagerMata)
        {
            GameObject rock = GameObject.Instantiate(boulder);
            rock.transform.SetParent(this.gameObject.transform);
            rock.transform.rotation = transform.rotation;
            rock.transform.position = transform.position;
        }
    }

    public void ReOpen()
    {
        isActive = true;
        if(transform.childCount > 0)
        {
            if (gameM is GameManagerMata)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
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
