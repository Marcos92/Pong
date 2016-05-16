using UnityEngine;
using System.Collections;

public class Batter : MonoBehaviour
{
    public Ball ballPrefab;
    public BaseballManager gameManager;
    public int health;
    bool damage;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball2")
        {
            damage = !damage;
            if (damage)
            {
                health--;
                gameManager.hud.UpdateScoresBaseball(gameManager.initialPoints, health);
                if (health <= 0)
                    gameManager.Won();
            }
            else
            {
                Vector3 position = other.transform.position;
                gameManager.balls.Remove(other.GetComponent<Ball>());
                Destroy(other.gameObject);
                Ball ball = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;
                gameManager.balls.Add(ball);
                System.Random r = new System.Random();
                int pongQuartets = gameManager.pongNumber / 4;
                int random = r.Next(0, 8 * pongQuartets);
                float angle = Mathf.PI / (8 * pongQuartets) + Mathf.PI / (4 * pongQuartets) * random;

                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 60;
                transform.forward = rb.velocity;
                ball.pongQuartets = pongQuartets;
                StartCoroutine(AlterBallSpeed(ball));
            }
        }
        else other.tag = "Ball2";
    }

    IEnumerator AlterBallSpeed(Ball ball)
    {
        float speed = ball.speed;
        ball.speed = 60;
        yield return null;
        ball.speed = speed;
    }
}
