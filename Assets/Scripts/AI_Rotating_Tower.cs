using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    bool isBallWay = false;
    public int team1Score = 0;
    public int team2Score = 0;
    Vector3 OldBallSpeed;

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

        else if(!toBall)
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

        if(BallCaptured && !isBallWay)
       {
            ThrowBall();
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
        transform.LookAt(Point.transform.position);
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
        transform.LookAt(ClosestBall.transform.position);

        Collider[] hitColliders = Physics.OverlapSphere(transform.gameObject.GetComponent<SphereCollider>().center, transform.gameObject.GetComponent<SphereCollider>().radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (ClosestBall.name == hitCollider.name)
            {
                OldBallSpeed = ClosestBall.GetComponent<Rigidbody>().velocity;
                ClosestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                AprouchBall = true;
               
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (toBall && !AprouchBall && ClosestBall.name == other.name)
        {
            OldBallSpeed = ClosestBall.GetComponent<Rigidbody>().velocity;
            ClosestBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            AprouchBall = true;
        }
    }

    void ToAprouchBall()
    {
        if (ClosestBall.transform.position != ((transform.forward * 1.5f) + transform.position))
        {
            ClosestBall.transform.position = Vector3.MoveTowards(ClosestBall.transform.position, ((transform.forward * 1.5f) + transform.position), 1);
        }

        else
        {
            BallCaptured = true;
            print("capturada");
        }
     }

    void ThrowBall()
    {
        Pong[] pongs = GameObject.FindObjectsOfType<Pong>();

        List<Pong> ValidPongs = new List<Pong>();
        Pong SelectedPlayer;
        

        int validTeam;

        if(team1Score>team2Score)
        {
            validTeam = 1;
        }

        else
        {
            validTeam = 2;
        }

        foreach (Pong pong in pongs)
        {
            if (pong.myTeam == validTeam)
            {
                ValidPongs.Add(pong);
            }
        
        }
        if (ValidPongs.Count == 0)
        {
            ClosestBall.GetComponent<Rigidbody>().velocity = OldBallSpeed;
            Destroy(gameObject);
        }

        int selectedInRangePlayer = (int)Random.Range(0, ValidPongs.Count);
        SelectedPlayer = ValidPongs[selectedInRangePlayer];

        transform.LookAt(SelectedPlayer.transform.position);
        ClosestBall.transform.position=((transform.forward * 1.5f) + transform.position);
        ClosestBall.transform.LookAt(SelectedPlayer.transform.position);
        ClosestBall.GetComponent<Rigidbody>().velocity = OldBallSpeed;
        isBallWay = true;
        print("Feito");
    }
}
