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
    Animator animator;

    public int points;

    //Bots
    public bool bot = false;
    public Transform nearestBall;
    public float delay;
    public float nextSearchTime;
    public float DelayedReaction;
    public float RamdomDistanceToDelay;
    public float dashCooldown;

    //Strike
    [HideInInspector]
    public bool striking = false;
    public float strikeDuration = 1f;
    public float strikePower;

    //Dash
    bool dashing = false;
    public float dashDuration = 0.5f;
    public float dashSpeedMultiplier = 1.2f;

    //[HideInInspector]
    public AudioClip hitBall, win;
    bool playOnce = false;
    AudioSource aSource;

    void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();

        animator = transform.GetChild(0).GetComponent<Animator>();
        
        
        

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
        delay = Random.Range(RamdomDistanceToDelay - delay, RamdomDistanceToDelay + delay);

        if (controlable) direction = (int)Input.GetAxisRaw("Horizontal");

        float velocity = direction * Time.deltaTime * speed;

        if (bot)
        {
            if (Time.time <= nextSearchTime)
            {
                botMoveToBallPosition();
            }

            else if (Time.time > nextSearchTime + delay)
            {
                nextSearchTime = Time.time + delay;
            }
        }
        else transform.Translate(transform.right * velocity, transform.parent);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
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
        dashCooldown = dashCooldown - Time.deltaTime;

        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(transform.localPosition.x, xMin, xMax);
        pos.z = Mathf.Clamp(transform.localPosition.z, zMin, zMax);
        transform.localPosition = pos;

        animator.SetInteger("direction", direction);
    }

    IEnumerator Strike()
    {
        striking = true;
        animator.SetBool("striking", striking);
        yield return new WaitForSeconds(strikeDuration);
        striking = false;
        animator.SetBool("striking", striking);
    }

    IEnumerator Dash(float d)
    {
        float endDashTime = Time.time + dashDuration;
        float v = d * Time.deltaTime * speed * dashSpeedMultiplier;

        dashing = true;
        animator.SetBool("dashing", dashing);

        while (Time.time < endDashTime)
        {
            transform.Translate(transform.right * v, transform.parent);
            yield return null;
        }

        dashing = false;
        animator.SetBool("dashing", dashing);
    }

    enum BotDirection { DirectionX, DirectionY };

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball" && !playOnce)
        {
            if (bot) StartCoroutine("Strike");
            playOnce = true;
            //aSource.clip = hitBall;
            //aSource.Play();
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ball" && playOnce)
        {
            playOnce = false;
        }
    }

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
                if (DistanceLeft < DistanceRight * 0.5 && DistanceLeft < DistanceForward * 0.95 && dashCooldown <= 0)
                {
                    StartCoroutine("Dash", -1);
                    dashCooldown = 2;
                }else
                    direction = -1;

            }

            else if (DistanceLeft > DistanceRight && DistanceRight < DistanceForward)
            {
                if (DistanceLeft > DistanceRight * 0.5 && DistanceRight < DistanceForward * 0.9 && dashCooldown <= 0)
                {
                    StartCoroutine("Dash", 1);
                    dashCooldown = 2;
                }else
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
