using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float speed = 30;
	Rigidbody rb;
	SphereCollider sc;
	int actualDirectionZ, actualDirectionX;
	float x, z;
	Vector3 dir;
    internal int pongQuartets = 4;
    internal bool cornerSpawn = false;
    private Pong lastHit = null;
    public GameManagerMata gManager = null;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();

        if(!cornerSpawn)
        {
            System.Random r = new System.Random();
            int random = r.Next(0, 8 * pongQuartets);
            float angle = Mathf.PI / (8 * pongQuartets) + Mathf.PI / (4 * pongQuartets) * random;

            //rb = GetComponent<Rigidbody>();
            //sc = GetComponent<SphereCollider>();

            rb.velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * speed;
            actualDirectionZ = 1;
            actualDirectionX = 1;
        }
        else
        {
            Vector3 myPos = transform.position;
            float angle = 0;
            float maxVariation = 30.0f;
            float offSet = UnityEngine.Random.Range(-maxVariation, maxVariation);
            if (myPos.x > 0 && myPos.z> 0)
            {
                angle = 225.0f;
            }
            else if (myPos.x > 0 && myPos.z < 0)
            {
                angle = 135.0f;
            }
            else if (myPos.x < 0 && myPos.z > 0)
            {
                angle = 315.0f;
            }
            else if (myPos.x < 0 && myPos.z < 0)
            {
                angle = 45.0f;
            }
            angle += offSet;
            float rads = Mathf.Deg2Rad * angle;
            rb.velocity = new Vector3(Mathf.Cos(rads), 0, Mathf.Sin(rads)) * speed;
        }
	}

	void OnCollisionEnter(Collision col)
    {
		if (col.gameObject.tag == "Goal") 
		{
			//Ignores collision and keeps same direction
			Physics.IgnoreCollision (sc, col.collider);
			rb.velocity = dir * speed;
		}
		else if (col.gameObject.tag == "Player")
		{
            Pong pongHit = col.gameObject.GetComponent<Pong>();
            int pongAngle = (int)col.transform.rotation.eulerAngles.y;

            if (pongAngle == 0)
            {
                x = hitFactor(transform.position.x, col.transform.position.x, col.collider.bounds.size.x);
                dir = new Vector3(x, 0, -actualDirectionZ).normalized;

                //reflects
                actualDirectionZ = inverseD(actualDirectionZ);
                rb.velocity = dir * speed;
            }

            else if (pongAngle > 0 && pongAngle < 90)
            {
                //Use X axis influence
                int valueToWork = 90 - pongAngle;

                //Cross Multiply
                float xInfluence = ((valueToWork * 100) / 90) / 100;
                float zInfluence = 1 - xInfluence;

                float resHitFactor = hitFactor(transform.position.x, col.transform.position.x, col.collider.bounds.size.x);

                float calculatedXFactor = resHitFactor * xInfluence;
                float calculatedZFactor = resHitFactor * zInfluence;


                if (calculatedZFactor > 0.0f)
                    dir = new Vector3(calculatedXFactor, 0, -actualDirectionZ * calculatedZFactor).normalized;
                else
                    dir = new Vector3(calculatedXFactor, 0, -actualDirectionZ).normalized;

                actualDirectionZ = inverseD(actualDirectionZ);
                rb.velocity = dir * speed;
            }

            else if (pongAngle >= 90 && pongAngle < 180)
            {
                //Use Z axis influence
                int valueToWork = 180 - pongAngle;

                //Cross Multiply
                float zInfluence = ((valueToWork * 100) / 90) / 100;
                float xInfluence = 1 - zInfluence;

                float resHitFactor = hitFactor(transform.position.z, col.transform.position.z, col.collider.bounds.size.z);

                float calculatedZFactor = resHitFactor * zInfluence;
                float calculatedXFactor = resHitFactor * xInfluence;

                if (calculatedXFactor > 0.0f)
                    dir = new Vector3(-actualDirectionX * calculatedXFactor, 0, calculatedZFactor).normalized;
                else
                    dir = new Vector3(-actualDirectionX, 0, calculatedZFactor).normalized;


                actualDirectionX = inverseD(actualDirectionX);
                rb.velocity = dir * speed;
            }

            else if (pongAngle >= 180 && pongAngle < 270)
            {
                //Use X axis influence
                int valueToWork = 270 - pongAngle;

                //Cross Multiply
                float xInfluence = ((valueToWork * 100) / 90) / 100;
                float zInfluence = 1 - xInfluence;

                float resHitFactor = hitFactor(transform.position.x, col.transform.position.x, col.collider.bounds.size.x);

                float calculatedXFactor = resHitFactor * xInfluence;
                float calculatedZFactor = resHitFactor * zInfluence;


                if (calculatedZFactor > 0.0f)
                    dir = new Vector3(calculatedXFactor, 0, -actualDirectionZ * calculatedZFactor).normalized;
                else
                    dir = new Vector3(calculatedXFactor, 0, -actualDirectionZ).normalized;


                actualDirectionZ = inverseD(actualDirectionZ);
                rb.velocity = dir * speed;
            }

            else if (pongAngle >= 270 && pongAngle < 360)
            {
                //Use Z axis influence
                int valueToWork = 360 - pongAngle;

                //Cross Multiply
                float zInfluence = ((valueToWork * 100) / 90) / 100;
                float xInfluence = 1 - zInfluence;

                float resHitFactor = hitFactor(transform.position.z, col.transform.position.z, col.collider.bounds.size.z);

                float calculatedZFactor = resHitFactor * zInfluence;
                float calculatedXFactor = resHitFactor * xInfluence;

                if (calculatedXFactor > 0.0f)
                    dir = new Vector3(-actualDirectionX * calculatedXFactor, 0, calculatedZFactor).normalized;
                else
                    dir = new Vector3(-actualDirectionX, 0, calculatedZFactor).normalized;


                actualDirectionX = inverseD(actualDirectionX);
                rb.velocity = dir * speed;
            }

            if (!pongHit.isMid)
            {
                lastHit = pongHit;
            }
            else
            {
                if (lastHit != null)
                {
                    if(pongHit.team != lastHit.team)
                    {
                        gManager.ScoreGoal(pongHit.team, false);
                    }
                    else
                    {
                        gManager.ScoreGoal(pongHit.team, true);
                    }
                    Destroy(this.gameObject);
                }
            }
		}
	}

	float hitFactor(float ballPos, float pongPos, float pongSize)
	{
		return (ballPos - pongPos) / pongSize;
	}

	int inverseD(int value)
	{
		if (value > 0)
			return -1;
		else {
			return 1;
		}
	}
}
