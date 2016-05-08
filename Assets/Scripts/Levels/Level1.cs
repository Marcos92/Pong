using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour
{
    public GameManager gameManager;
    public float timeBetweenQuakes, timeBetweenRocks;
    float nextQuakeTime, nextRockTime;
    public int numberOfRocks;
    public Rock rockPrefab;
    float ray;

	void Start ()
    {
        nextQuakeTime = Time.time + timeBetweenQuakes;

        ray = gameManager.ray * 0.75f; //Evita que as rochas façam spawn nas extremidades do campo
	}
	
	void Update ()
    {
	    if(Time.time >= nextQuakeTime)
        {
            nextQuakeTime = Time.time + nextQuakeTime + numberOfRocks * timeBetweenRocks;

            StartCoroutine("GenerateRocks");
        }
	}

    IEnumerator GenerateRocks()
    {
        int counter = numberOfRocks;

        while(counter > 0)
        {
            Rock r = Instantiate(rockPrefab, new Vector3(Random.Range(-ray,ray), 50f, Random.Range(-ray, ray)), transform.rotation) as Rock;
            counter--;
            yield return new WaitForSeconds(timeBetweenRocks * Random.Range(0.5f, 1.5f)); //Evita que o spawn das rochas não seja regular
        }

        StopAllCoroutines();
    }
}
