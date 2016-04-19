using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject pong;
    public int pongNumber;
    public float ray;
    public float defaultSpeed;
    public float limit;
    public float pongLength;

	// Use this for initialization
	void Start ()
    {
        float f = 360 / pongNumber;
        Quaternion q = Quaternion.Euler(new Vector3(0, f));
        Vector3 offset = new Vector3(0, 0, -1) * ray;
        offset.y += 1.5f;

        float side = 2 * ray * Mathf.Sin(Mathf.PI / pongNumber) - pongLength - limit;
        for (int i = 0; i < pongNumber; i++)
        {
            GameObject o = (GameObject)Instantiate(pong, transform.position + offset, Quaternion.Euler(new Vector3(0, f * i)));
            o.transform.parent = transform;
            offset = q * offset;
            Controller c = o.GetComponent<Controller>();
            c.speed = defaultSpeed;
            c.limit = side / 2;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
