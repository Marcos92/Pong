using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject pongPrefab;
    public GameObject cornerPrefab;
    public bool pongs8;
    int pongNumber = 4;
    public float ray;
    public float defaultSpeed;

	// Use this for initialization
	void Start ()
    {
        Vector3 pongOffset = new Vector3(0, 0, -1) * ray;
        Vector3 cornerOffset = new Vector3(1, 0, 1) * ray;

        if (pongs8)
            pongNumber = 8;

        float angle = 360 / pongNumber;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, angle));

        if (pongs8)//quando são oito pongs é preciso ajustar o offset dos cantos
        {
            cornerOffset *= 3f / 4f;
            Quaternion halfRotation = Quaternion.Euler(new Vector3(0, angle / 2));
            cornerOffset = halfRotation * cornerOffset;
        }

        cornerOffset.y = pongOffset.y += 1.5f;
        
        for (int i = 0; i < pongNumber; i++)
        {
            GameObject pong = (GameObject)Instantiate(pongPrefab, transform.position + pongOffset, Quaternion.Euler(new Vector3(0, angle * i)));
            pong.transform.parent = transform;
            Controller controller = pong.GetComponent<Controller>();
            controller.speed = defaultSpeed;

            Instantiate(cornerPrefab, transform.position + cornerOffset, Quaternion.identity);

            pongOffset = rotation * pongOffset;
            cornerOffset = rotation * cornerOffset;

            if (i == 0) pong.transform.GetComponent<Controller>().enabled = true;
        }
    }
	
	void Update ()
    {
	
	}
}
