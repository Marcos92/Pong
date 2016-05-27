using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 30;
    Rigidbody rb;
    BoxCollider bc;
    Vector3 dir;
    public int team; //0 or 1
    // Use this for initialization
    void Start () {
    }

    public void Shot(Vector3 direction, int t)
    {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        dir = direction;
        rb.velocity = dir * speed;
        team = t;
    }

    void OnCollisionEnter(Collision col)
    {
        //Marco, chama aqui o que quiseres
        //Nota, usa, o EnemyAIDog como prefab ou então se criares um novo, no inspector desse novo prefab seleciona a layer EnemyAIDog
    }
}
