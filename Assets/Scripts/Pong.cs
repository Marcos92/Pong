﻿using UnityEngine;
using System.Collections;

public class Pong : MonoBehaviour
{
    public float speed;
    float initialSpeed;
    internal int direction, directionY;
    public bool controlable = false;
    [HideInInspector]
    public float bounds;
    public float xMin, xMax, zMin, zMax;
    public float midXMin, midXMax, midZmin, midZMax;
    [HideInInspector]
    public Goal goal;
    //Rigidbody rigidBody;
    Animator animator;
    public Bullet DebugBulletPrefab;

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
    public AudioClip win, hitBall, strikeSound,strikeSound2;
    bool playOnce = false;
    AudioSource aSource, strikeSource, hitBallSource, strikeSource2;
    int rndStrike = 0;
    

    //Mata
    public bool isMid;
    public Vector3 oldPos;
    public Quaternion oldRotation;
    public int team;

    //Shots
    public bool canShoot;
    public float timeBetweenShots;
    private float timeToNextShot;
    public int myTeam;
    public bool isDebug = false;

    void Start()
    {
        initialSpeed = speed;

        aSource = GetComponent<AudioSource>();
        hitBallSource = GetComponent<AudioSource>();
        strikeSource = GetComponent<AudioSource>();
        strikeSource2 = GetComponent<AudioSource>();
        
        hitBallSource.clip = hitBall;

        

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
        if (controlable)
        {
            direction = (int)Input.GetAxisRaw("Horizontal");
            if (isMid) directionY = (int)Input.GetAxisRaw("Vertical");
        }

        float velocity = 0;
        if(!dashing) velocity = direction * Time.deltaTime * speed;

        if (!isMid)
        {
            if (bot)
            {
                delay = Random.Range(RamdomDistanceToDelay - delay, RamdomDistanceToDelay + delay);

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
        }
        else
        {
            if (bot)
            {
                delay = Random.Range(RamdomDistanceToDelay - delay, RamdomDistanceToDelay + delay);

                if (Time.time <= nextSearchTime)
                {
                    botMoveToBallPosition();
                }

                else if (Time.time > nextSearchTime + delay)
                {
                    nextSearchTime = Time.time + delay;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && controlable)
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                //Strike

                StartCoroutine("Strike");
                strikeSource.clip = strikeSound;
                strikeSource.Play();
                
            }
            else
            {
                //Dash
                StartCoroutine("Dash", Input.GetAxisRaw("Horizontal"));
                rndStrike = Random.Range(0, 3);

                if (rndStrike == 1)
                {
                    strikeSource.clip = strikeSound;
                    strikeSource.Play();
                    Debug.Log("1");
                }
                if(rndStrike == 2)
                {
                    strikeSource2.clip = strikeSound2;
                    strikeSource2.Play();
                    Debug.Log("2");
                }
            }
        }
        dashCooldown = dashCooldown - Time.deltaTime;

        if (!isMid)
        {
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Clamp(transform.localPosition.x, xMin, xMax);
            pos.z = Mathf.Clamp(transform.localPosition.z, zMin, zMax);
            transform.localPosition = pos;
        }
        else
        {
            transform.localPosition += new Vector3(direction, 0, directionY) * speed * Time.deltaTime;
            Vector3 nextPos = transform.position;
            nextPos.x = Mathf.Clamp(transform.localPosition.x, midXMin, midXMax);
            nextPos.z = Mathf.Clamp(transform.localPosition.z, midZmin, midZMax);
            transform.localPosition = nextPos;
        }
        
        animator.SetBool("dashing", dashing);
        animator.SetBool("striking", striking);
        animator.SetInteger("direction", direction);

        ShootTest();
    }

    void ShootTest()
    {
        timeToNextShot -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G) && controlable)
        {
            if (canShoot && timeToNextShot <= 0)
            {
                timeToNextShot = timeBetweenShots;

                //SpawnBullet
                if (isDebug && DebugBulletPrefab)
                {
                    Bullet bullet = Instantiate(DebugBulletPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Bullet;
                    bullet.Shot(transform.forward, myTeam);
                }

                else
                {
                    this.transform.parent.gameObject.GetComponent<GameManager>().SpawnBullet(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward, myTeam);
                }
          }
        }
    }

    IEnumerator Strike()
    {
        striking = true;
        speed = 0f;
        yield return new WaitForSeconds(strikeDuration);
        striking = false;
        speed = initialSpeed;
        
    }

    IEnumerator Dash(float d)
    {
        float endDashTime = Time.time + dashDuration;
        float v = d * Time.deltaTime * speed * dashSpeedMultiplier;

        dashing = true;

        while (Time.time < endDashTime)
        {
            transform.Translate(transform.right * v, transform.parent);
            yield return null;
        }

        dashing = false;
        
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

    public virtual void botMoveToBallPosition()
    {
        GameObject go = GameObject.Find("Ball(Clone)");
        int directionForward = 0;
        if (go)
        {
            nearestBall = go.transform;
            Vector3 vectorLeftTest = ((transform.right * -1) * 2) + transform.position;
            Vector3 vectorRightTest = (transform.right * 2) + transform.position;
            Vector3 vectorForwardTest = (transform.forward * 2) + transform.position;
            Vector3 vectorBackwardTest = ((transform.forward * -1) * 2) + transform.position;

            Debug.DrawLine(transform.position, vectorLeftTest, Color.blue);
            Debug.DrawLine(transform.position, vectorRightTest, Color.blue);

            float DistanceLeft = Vector3.Distance(nearestBall.position, vectorLeftTest);
            float DistanceRight = Vector3.Distance(nearestBall.position, vectorRightTest);
            float DistanceNeutral = Vector3.Distance(nearestBall.position, transform.position);
            float DistanceBackward = Vector3.Distance(nearestBall.position, vectorBackwardTest);
            float DistanceForward = Vector3.Distance(nearestBall.position, vectorForwardTest);

            if (!isMid)
            {
                if (DistanceLeft < DistanceRight && DistanceLeft < DistanceNeutral)
                {
                    if (DistanceLeft < DistanceRight * 0.5 && DistanceLeft < DistanceNeutral * 0.95 && dashCooldown <= 0)
                    {
                        StartCoroutine("Dash", -1);
                        dashCooldown = 2;
                    }
                    else
                        direction = -1;

                }

                else if (DistanceLeft > DistanceRight && DistanceRight < DistanceNeutral)
                {
                    if (DistanceLeft > DistanceRight * 0.5 && DistanceRight < DistanceNeutral * 0.9 && dashCooldown <= 0)
                    {
                        StartCoroutine("Dash", 1);
                        dashCooldown = 2;
                    }
                    else
                        direction = 1;
                }

                else
                {
                    direction = 0;
                }
            }
            else
            {
                if (DistanceRight < DistanceLeft)
                {
                    if (DistanceLeft > DistanceRight && DistanceRight < DistanceNeutral * 0.95 && dashCooldown <= 0)
                    {
                        StartCoroutine("Dash", -1);
                        dashCooldown = 2;
                    }
                    else
                        direction = -1;
                }

                else
                {
                    if (DistanceLeft < DistanceRight && DistanceLeft < DistanceNeutral * 0.95 && dashCooldown <= 0)
                    {
                        StartCoroutine("Dash", 1);
                        dashCooldown = 2;
                    }
                    else
                        direction = 1;
                }


                if (DistanceBackward < DistanceForward)
                {
                    directionForward = 1;
                }

                else
                {
                    directionForward = -1;
                }
            }

            float velocity = direction * Time.deltaTime * speed;
            float velocityForward = directionForward * Time.deltaTime * speed;

            transform.Translate(transform.right * velocity, transform.parent);
            if (isMid) transform.Translate(transform.forward * velocityForward, transform.parent);
            //print(velocity + " " + DistanceNeutral + " " + DistanceLeft + " " + DistanceRight);
        }
    }
}
