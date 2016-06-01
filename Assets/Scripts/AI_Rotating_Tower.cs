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
    bool AprouchBall = false;
    bool BallCaptured = false;
    int team1Score = 0;
    int team2Score = 0;
    

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

       if(toBall && !AprouchBall)
        {
            
            // float distance = Vector3.Magnitude( - );
            //float distance = Mathf.Sqrt((ClosestBall.transform.position - transform.position).sqrMagnitude);
            //Vector3 dir = ClosestBall.transform.position - transform.position;
            //float length = dir.magnitude;
            //Vector3 heading = ClosestBall.transform.position - transform.position;
            //float distance = Vector3.Distance(ClosestBall.transform.position, transform.position);
            GoToBall();
        }

       else if(AprouchBall && !BallCaptured)
       {
            ToAprouchBall();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.gameObject.GetComponent<SphereCollider>().center, transform.gameObject.GetComponent<SphereCollider>().radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (ClosestBall.name == hitCollider.name)
            {
                ClosestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                AprouchBall = true;
               
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (toBall && !AprouchBall && ClosestBall.name == other.name)
        {
            ClosestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            AprouchBall = true;
            print("entrou");
        }
    }

    void ToAprouchBall()
    {
        if (ClosestBall.transform.position != ((transform.forward * 0.5f) + transform.position))
        {
            ClosestBall.transform.position = Vector3.MoveTowards(ClosestBall.transform.position, ((transform.forward * 1.5f) + transform.position), 1);
        }

        else
        {
           
        }
     }
}
