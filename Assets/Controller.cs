using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    internal float speed;
    internal int direction;
    internal int corner = 0;
    Rigidbody rigidBody;

	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        Vector3 v = Quaternion.Inverse(transform.localRotation) * transform.localPosition;
        direction = (int)Input.GetAxisRaw("Horizontal");
        if (corner != direction)
        {
            float velocity = direction * Time.deltaTime * speed;
            rigidBody.AddForce(transform.right * direction);
            
            //transform.Translate(transform.right * velocity, transform.parent);
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
