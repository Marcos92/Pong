using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Pong owner;
    public GameManager gameM;
    public Score scoreManager;
    private bool isActive = true;
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
                gameM.SpawnBall();
            }
        }
    }
}
