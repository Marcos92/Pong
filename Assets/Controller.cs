using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    internal float speed;
    internal float limit;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 v = Quaternion.Inverse(transform.localRotation) * transform.localPosition;
        float direction = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        if (v.x * Mathf.Sign(direction) < limit)
            transform.Translate(transform.right * direction, transform.parent);
    }
}
