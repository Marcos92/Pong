using UnityEngine;
using System.Collections;

public class Pong : MonoBehaviour
{
    public float speed;
    internal int direction;
    public bool controlable = false;
    [HideInInspector]
    public float bounds;
    [HideInInspector]
    public Goal goal;
    //Rigidbody rigidBody;

    public int points;

    //Bots
    public bool bot = false;
    Transform nearestBall;
    public float delay;
    float nextSearchTime;

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
        xMin = initialPosition.x - myBounds * right.x;
        xMax = initialPosition.x + myBounds * right.x;
        if (xMin > xMax)
        {
            aux = xMin;
            xMin = xMax;
            xMax = aux;
        }
        zMin = initialPosition.z + myBounds * right.z;
        zMax = initialPosition.z - myBounds * right.z;
        if (zMin > zMax)
        {
            aux = zMin;
            zMin = zMax;
            zMax = aux;
        }
        //print(bounds);

        nextSearchTime = Time.time;
    }
    float xMin, xMax, zMin, zMax;
    public static float myBounds;
    void Update ()
    {
        if(controlable) direction = (int)Input.GetAxisRaw("Horizontal");

        float velocity = direction * Time.deltaTime * speed;

        if (bot) 
        {
            if (Time.time > nextSearchTime)
            {
                nextSearchTime = Time.time + delay;

                //Guardar posição da bola mais próxima
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
}
