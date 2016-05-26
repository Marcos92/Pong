using UnityEngine;
using System.Collections;

public class PongCircularMove : Pong
{
    public float minDegrees, maxDegrees;

    public float distanceToCenter;
    public float currentAngle;
    private bool lookingAtCenter;
    private int directionX, directionZ;

    public bool lookAtCenter;
    public int team;

    public float moveRadius;
    public bool isMid;


    void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
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
        MoveCircular();
    }

    void MoveCircular()
    {
        if (!isMid)
        {
            if (bot)
            {
                if (Time.time <= nextSearchTime)
                {
                    botMoveToBallPosition();
                    //Guardar posição da bola mais próxima
                }

                else if (Time.time > nextSearchTime + delay)
                {
                    nextSearchTime = Time.time + delay;
                }
            }

            else
            {
                if (controlable) direction = (int)Input.GetAxisRaw("Horizontal");

                //float tempAngle = currentAngle + direction * speed;
                //if (tempAngle >= minDegrees && tempAngle <= maxDegrees)
                //    currentAngle += direction * speed;

                float velocity = direction * Time.deltaTime * speed;

                float tempAngle = currentAngle + velocity;

                if (tempAngle >= minDegrees && tempAngle <= maxDegrees)
                    currentAngle += velocity;


                float degrees = currentAngle * Mathf.PI / 180;
                float x = Mathf.Cos(degrees) * distanceToCenter;
                float z = Mathf.Sin(degrees) * distanceToCenter;

                transform.position = new Vector3(x, transform.position.y, z);
                if (lookAtCenter)
                {
                    //if (!lookingAtCenter)
                    //{
                    //    transform.LookAt(Vector3.zero);
                    //    lookingAtCenter = true;
                    //}
                    transform.LookAt(Vector3.zero);
                    lookingAtCenter = true;
                }
                else transform.rotation = Quaternion.identity;
            }

        }
        else
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

    public override void botMoveToBallPosition()
    {
        delay = Random.Range(RamdomDistanceToDelay - delay, RamdomDistanceToDelay + delay);
        print("ok");
        //GameObject go = GameObject.Find("Ball(Clone)");
        GameObject go = GameObject.FindGameObjectWithTag("Ball");
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

            float tempAngle = currentAngle + velocity;

            if (tempAngle >= minDegrees && tempAngle <= maxDegrees)
                currentAngle += velocity;

            float degrees = currentAngle * Mathf.PI / 180;
            float x = Mathf.Cos(degrees) * distanceToCenter;
            float z = Mathf.Sin(degrees) * distanceToCenter;

            transform.position = new Vector3(x, transform.position.y, z);

            if (lookAtCenter)
            {
                transform.LookAt(Vector3.zero);
                lookingAtCenter = true;
            }
            else transform.rotation = Quaternion.identity;

            //transform.Translate(transform.right * velocity, transform.parent);
            //print(velocity + " " + DistanceForward + " " + DistanceLeft + " " + DistanceRight);
        }
    }
}
