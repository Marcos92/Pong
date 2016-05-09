using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Pong owner;
    public GameManager gameM;
    AudioSource aSource;
    public AudioClip goalClip;
    public Score scoreManager;
    private bool isActive = true;

	void Start () {
	    aSource = GetComponent<AudioSource>();
	}
	

	void Update () {
	    
	}

    void ScoreGoal()
    {
        owner.points -= 1;
        aSource.clip = goalClip;
        aSource.Play();
        scoreManager.UpdateScores();
    }

    public void CloseGoal()
    {
        isActive = false;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.rotation = transform.rotation;
        cube.transform.position = transform.position;
        cube.transform.localScale = GetComponent<BoxCollider>().size;
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
