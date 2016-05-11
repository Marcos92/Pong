using UnityEngine;
using System.Collections;

public class WayPointRotatingTower : MonoBehaviour {

    public int WayPointReference;


    void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
