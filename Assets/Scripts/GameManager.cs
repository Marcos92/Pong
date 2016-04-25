using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Pong pongPrefab;
    public GameObject cornerPrefab;
    public Goal goalPrefab;
    public bool pongs8;
    int pongNumber = 4;
    public float ray;

	// Use this for initialization
	void Start ()
    {
        Vector3 pongOffset = new Vector3(0, 0, -1) * ray;
        Vector3 cornerOffset = new Vector3(1, 0, -1) * ray;

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
            Pong pong = Instantiate(pongPrefab, transform.position + pongOffset, Quaternion.Euler(new Vector3(0, angle * i))) as Pong;
            pong.transform.parent = transform;

            Instantiate(cornerPrefab, transform.position + cornerOffset, Quaternion.identity);
            pong.bounds = Vector3.Distance(pong.transform.position, transform.position + cornerOffset) - pong.transform.GetComponent<Renderer>().bounds.size.x * 0.5f;
            //^Ainda não funciona com os 8 jogadores

            //Atribuir baliza ao pong
            Goal goal = Instantiate(goalPrefab, pong.transform.position - pong.transform.forward * 3f, Quaternion.Euler(new Vector3(0, angle * i))) as Goal;
            goal.transform.GetComponent<BoxCollider>().size = new Vector3(pong.bounds * 2.5f, 0, 5f);
            pong.goal = goal;

            if (i == 0) pong.controlable = true; //Mudar conforme o jogador

            pongOffset = rotation * pongOffset;
            cornerOffset = rotation * cornerOffset;
        }
    }
	
	void Update ()
    {
	
	}
}
