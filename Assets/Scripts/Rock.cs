using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class Rock : MonoBehaviour
{
    public float fallSpeed = 5f;

    void Start()
    {

    }

    void Update()
    {
        if (transform.position.y > GetComponent<MeshRenderer>().bounds.size.y/2) transform.position -= Vector3.up * fallSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        Ball b = other.gameObject.GetComponent<Ball>();
        if (b)
        {
            Destroy(gameObject); //Destruir se for atingido por uma bola
        }
    }
}
