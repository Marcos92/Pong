using UnityEngine;
using System.Collections;

public class AI_Rotating_Tower : MonoBehaviour {

    int NextWayPoint = 0;
    public int topWayPoint;
    WayPointRotatingTower Point;
    // Use this for initialization
    public float movementSpeed=2;
    Ball ClosestBall;
    bool toBall = false;

    void Start () {
        FindMyNextWayPoint();
        
	}
	
	// Update is called once per frame
	void Update () {
	   if(Vector3.Distance(transform.position, Point.transform.position)<=0.5 && !toBall)
       {
            if(NextWayPoint+1>topWayPoint)
            {
                findNearestBall();
            }

            else
            {
                NextWayPoint++;
                FindMyNextWayPoint();
                
            }
       }

       else
        {
            GoToWayPoint();
        }

       if(toBall)
        {
            if(Vector3.Distance(transform.position, ClosestBall.transform.position)<= 1)
            {
                ClosestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                print("entrei!!!");
            }

            else
            {
                GoToBall();
            }
        }
	}

    void FindMyNextWayPoint()
    {
        WayPointRotatingTower [] wayPoints = GameObject.FindObjectsOfType<WayPointRotatingTower>();

        foreach(WayPointRotatingTower waypoint in wayPoints )
        {
            if(waypoint.WayPointReference == NextWayPoint)
            {
                Point = waypoint;
            }
        }
    }

    void GoToWayPoint()
    {
       transform.position=Vector3.MoveTowards(transform.position, Point.transform.position, movementSpeed);
    }

    void findNearestBall()
    {
        Ball [] balls = GameObject.FindObjectsOfType<Ball>();

        ClosestBall = balls[0];

        foreach (Ball ball in balls)
        {
            if(Vector3.Distance(ball.transform.position,transform.position)< Vector3.Distance(ClosestBall.transform.position, transform.position))
            {
                ClosestBall = ball;
            }
        }

        toBall = true;
    }


    void GoToBall()
    {
        movementSpeed= movementSpeed * 1.05f;
        transform.position = Vector3.MoveTowards(transform.position, ClosestBall.transform.position, movementSpeed);
    }
}
