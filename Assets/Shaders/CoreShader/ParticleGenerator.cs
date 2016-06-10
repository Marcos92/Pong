using UnityEngine;
using System.Collections;

public class ParticleGenerator : MonoBehaviour
{
    public int boundsX, boundsZ;
    public SphericalFog particle;
    public float timeBetweenParticles;
    float nextParticleSpawn;

	void Start ()
    {
        nextParticleSpawn = Time.time + timeBetweenParticles;
	}
	
	void Update ()
    {
	    if(Time.time >= nextParticleSpawn)
        {
            nextParticleSpawn = Time.time + timeBetweenParticles;

            SphericalFog p = Instantiate(particle, new Vector3(Random.Range(-boundsX, boundsX), 0, Random.Range(-boundsZ, boundsZ)), transform.rotation) as SphericalFog;
        }
	}
}
