using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour
{
    public GameManager gameManager;
    public float timeBetweenQuakes, timeBetweenRocks, shakeDuration;
    float nextQuakeTime, nextRockTime;
    public int numberOfRocks;
    public Rock rockPrefab;
    float ray;
    public float shakeIntensity;
    public AudioClip quake;
    AudioSource audioSource;
    bool playOnce = false;


	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

        nextQuakeTime = Time.time + timeBetweenQuakes;

        ray = gameManager.ray * 0.75f; //Evita que as rochas façam spawn nas extremidades do campo
	}
	
	void Update ()
    {
	    if(Time.time >= nextQuakeTime)
        {
            if (!playOnce)
            {
                playOnce = true;
                audioSource.clip = quake;
                audioSource.Play();
            }
            nextQuakeTime = Time.time + nextQuakeTime + numberOfRocks * timeBetweenRocks;

            StartCoroutine("GenerateRocks");
            StartCoroutine("ShakeCamera");
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

        StopCoroutine("GenerateRocks");
    }

    IEnumerator ShakeCamera()
    {
        float stopShakeTime = Time.time + shakeDuration;
        Vector3 cameraInitialPosition = Camera.main.transform.position;

        while(Time.time < stopShakeTime)
        {
            Camera.main.transform.position = new Vector3(cameraInitialPosition.x + Random.Range(-shakeIntensity, shakeIntensity), cameraInitialPosition.y + Random.Range(-shakeIntensity, shakeIntensity), cameraInitialPosition.z);
            yield return null;
        }

        Camera.main.transform.position = cameraInitialPosition;
        StopCoroutine("ShakeCamera");
    }
}
