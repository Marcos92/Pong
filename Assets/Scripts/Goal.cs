using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Pong owner;
    public GameManager gameM;
    public Score scoreManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void ScoreGoal()
    {
        owner.points -= 1;
        scoreManager.UpdateScores();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            ScoreGoal();
            Destroy(other.gameObject);
            gameM.SpawnBall();
        }
    }
}
