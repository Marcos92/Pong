using UnityEngine;
using System.Collections;

public class PongMid : PongCircularMove
{

    internal int direction;
    private int directionX, directionZ;
    bool playOnce = false;
    AudioSource aSource;
    public float moveRadius;

    void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();
        Vector3 right = transform.right;
        Vector3 initialPosition = transform.localPosition;
        float aux;
        xMin = initialPosition.x - bounds * right.x;
        xMax = initialPosition.x + bounds * right.x;
        if (xMin > xMax)
        {
            aux = xMin;
            xMin = xMax;
            xMax = aux;
        }
        zMin = initialPosition.z + bounds * right.z;
        zMax = initialPosition.z - bounds * right.z;
        if (zMin > zMax)
        {
            aux = zMin;
            zMin = zMax;
            zMax = aux;
        }

        nextSearchTime = Time.time;
    }

    void Update()
    {
        if (controlable)
        {
            directionX = (int)Input.GetAxisRaw("Horizontal");
            directionZ = (int)Input.GetAxisRaw("Vertical");
        }

        float velocityX = directionX * Time.deltaTime * speed;
        float velocityZ = directionZ * Time.deltaTime * speed;
        Vector3 newPosition = transform.position + new Vector3(velocityX, 0.0f, velocityZ);

        if (newPosition.x >= -moveRadius && newPosition.x <= moveRadius)
            transform.position += new Vector3(velocityX, 0.0f, 0.0f);
        if (newPosition.z >= -moveRadius && newPosition.z <= moveRadius)
            transform.position += new Vector3(0.0f, 0.0f, velocityZ);

    }

    IEnumerator Strike()
    {
        striking = true;
        yield return new WaitForSeconds(1f);
        striking = false;
    }

    IEnumerator Dash(float d)
    {
        float endDashTime = Time.time + dashDuration;
        float v = d * Time.deltaTime * speed * dashSpeedMultiplier;

        while (Time.time < endDashTime)
        {
            transform.Translate(transform.right * v, transform.parent);
            yield return null;
        }
    }

    enum BotDirection { DirectionX, DirectionY };

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball" && !playOnce)
        {
            playOnce = true;
            aSource.clip = hitBall;
            aSource.Play();
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ball" && playOnce)
        {
            playOnce = false;
        }
    }

    void botMoveToBallPosition()
    {
        GameObject go = GameObject.Find("Ball(Clone)");
        if (go)
        {
            nearestBall = go.transform;
            Vector3 vectorLeftTest = ((transform.right * -1) * 2) + transform.position;
            Vector3 vectorRightTest = (transform.right * 2) + transform.position;

            Debug.DrawLine(transform.position, vectorLeftTest, Color.blue);
            Debug.DrawLine(transform.position, vectorRightTest, Color.blue);

            float DistanceLeft = Vector3.Distance(nearestBall.position, vectorLeftTest);
            float DistanceRight = Vector3.Distance(nearestBall.position, vectorRightTest);
            float DistanceForward = Vector3.Distance(nearestBall.position, transform.position);

            if (DistanceLeft < DistanceRight && DistanceLeft < DistanceForward)
            {
                direction = -1;

            }

            else if (DistanceLeft > DistanceRight && DistanceRight < DistanceForward)
            {
                direction = 1;
            }

            else
            {
                direction = 0;
            }

            float velocity = direction * Time.deltaTime * speed;

            transform.Translate(transform.right * velocity, transform.parent);
            //print(velocity + " " + DistanceForward + " " + DistanceLeft + " " + DistanceRight);
        }
    }
}
