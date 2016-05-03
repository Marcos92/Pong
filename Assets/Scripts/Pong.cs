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
        print(bounds);
    }
    float xMin, xMax, zMin, zMax;
    public static float myBounds;
    void Update ()
    {
        if(controlable) direction = (int)Input.GetAxisRaw("Horizontal");

        float velocity = direction * Time.deltaTime * speed;
        transform.Translate(transform.right * velocity, transform.parent);

        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(transform.localPosition.x, xMin, xMax);
        pos.z = Mathf.Clamp(transform.localPosition.z, zMin, zMax);
        transform.localPosition = pos;
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
