using UnityEngine;
using System.Collections;

public class TestAI : MonoBehaviour {

    public float enemyMovementSpeed;
    public Transform Target;
    Rigidbody theRigidBody;
    Renderer myRenderer;//Nao sei se vou precisar disto *1
    public bool fixedXAxis;
    Vector3 MyInitialPosition;
    public float MaxTravelDistanceBetweenInitialPosition;
    public float DelayedReaction;
    float travelDistanceBetweenInitialPositionX;
    float travelDistanceBetweenInitialPositionZ;
    // Use this for initialization
    void Start ()
    {
        myRenderer = GetComponent<Renderer>();
        theRigidBody = GetComponent<Rigidbody>();
        MyInitialPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Vector3.Distance(MyInitialPosition, transform.position);

        if (fixedXAxis)
        {
            travelDistanceBetweenInitialPositionX =  MyInitialPosition.x - Target.transform.position.x;

            if(travelDistanceBetweenInitialPositionX < 0)
            {
                travelDistanceBetweenInitialPositionX = travelDistanceBetweenInitialPositionX * -1; 
            }

            if (travelDistanceBetweenInitialPositionX < MaxTravelDistanceBetweenInitialPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.position.x,transform.position.y,transform.position.z),DelayedReaction);
            }
        }

        else
        {
            travelDistanceBetweenInitialPositionZ = MyInitialPosition.z - Target.transform.position.z;

            if (travelDistanceBetweenInitialPositionZ < 0)
            {
                travelDistanceBetweenInitialPositionZ = travelDistanceBetweenInitialPositionZ * -1;
            }

            if (travelDistanceBetweenInitialPositionZ < MaxTravelDistanceBetweenInitialPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position,new Vector3(transform.position.x, transform.position.y, Target.transform.position.z),DelayedReaction);
            }
        }


        
	}
}
