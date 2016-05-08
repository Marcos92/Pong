using UnityEngine;
using System.Collections;

public class Batter : MonoBehaviour
{
    public Ball ballPrefab;
    public GameManager gameManager;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Vector3 position = other.transform.position;
            Destroy(other.gameObject);
            gameManager.balls.Remove(other.GetComponent<Ball>());
            Ball ball = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;
            gameManager.balls.Add(ball);
            System.Random r = new System.Random();
            int pongQuartets = gameManager.pongNumber / 4;
            int random = r.Next(0, 8 * pongQuartets);
            float angle = Mathf.PI / (8 * pongQuartets) + Mathf.PI / (4 * pongQuartets) * random;

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 60;
            ball.pongQuartets = pongQuartets;
        }
        else other.tag = "Ball";
    }
}
