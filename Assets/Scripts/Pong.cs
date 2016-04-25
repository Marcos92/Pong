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

	void Start ()
    {
        //rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        if(controlable) direction = (int)Input.GetAxisRaw("Horizontal");

        float velocity = direction * Time.deltaTime * speed;
        transform.Translate(transform.right * velocity, transform.parent);

        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(transform.localPosition.x, -bounds, bounds);
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
