using UnityEngine;
using System.Collections;

public class Pong : MonoBehaviour
{
    public float speed;
    internal int direction;
    public bool controlable = false;
    [HideInInspector]
    public float bounds;
    public float xMin, xMax, zMin, zMax;
    [HideInInspector]
    public Goal goal;
    //Rigidbody rigidBody;

    public int points;

    //Bots
    public bool bot = false;
    public Transform nearestBall;
    public float delay;
    public float nextSearchTime;
    public float DelayedReaction;
    

    //Strike
    [HideInInspector]
    public bool striking = false;
    public float strikeDuration = 1f;
    public float strikePower;

    //Dash
    public float dashDuration = 0.5f;
    public float dashSpeedMultiplier = 1.2f;

	void Start ()
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

    void Update ()
    {
        if(controlable) direction = (int)Input.GetAxisRaw("Horizontal");

        float velocity = direction * Time.deltaTime * speed;

        if (bot) 
        {
            if (Time.time <= nextSearchTime)
            {
                botMoveToBallPosition();
                //Guardar posição da bola mais próxima
            }
            
            else if(Time.time > nextSearchTime + delay)
            {
                nextSearchTime = Time.time + delay; 
            }

            //Movimento para a direcção guardada
        }
        else transform.Translate(transform.right * velocity, transform.parent);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Input.GetAxisRaw("Horizontal") == 0)
            {
                //Strike
                StartCoroutine("Strike");
            }
            else
            {
                //Dash
                StartCoroutine("Dash", Input.GetAxisRaw("Horizontal"));
            }
        }

        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(transform.localPosition.x, xMin, xMax);
        pos.z = Mathf.Clamp(transform.localPosition.z, zMin, zMax);
        transform.localPosition = pos;
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

        while(Time.time < endDashTime)
        {
            transform.Translate(transform.right * v, transform.parent);
            yield return null;
        }
    }

   enum BotDirection { DirectionX, DirectionY};

    //void OnTriggerEnter(Collider collider)
    //{
    //    print("enter");
    //    if (collider.tag == "Untagged")
    //    {
    //        corner = direction;
    //    }
    //}

    //void OnTriggerExit(Collider collider)
    //{
    //    print("leave");
    //    if (collider.tag == "Untagged")
    //    {
    //        corner = 0;
    //    }
    //}

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
