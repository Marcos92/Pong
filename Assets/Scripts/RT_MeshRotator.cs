using UnityEngine;
using System.Collections;

public class RT_MeshRotator : MonoBehaviour {


    public Transform target;
    public float speed;

    // Use this for initialization
    void Start () {
	
	}

    
    void Update()
    {
        //Vector3 targetDir = target.position - transform.position;
        //float step = speed * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        //Debug.DrawRay(transform.position, newDir, Color.red);
        //transform.rotation = Quaternion.LookRotation(newDir);
    }
}
