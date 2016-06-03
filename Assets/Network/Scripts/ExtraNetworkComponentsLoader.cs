using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ExtraNetworkComponentsLoader : MonoBehaviour
{
    public Animator TargetAnimator; // For the network animator

    bool active = true;

    void Update()
    {
        if (active)
        {
            if (transform.parent != null && transform.parent.gameObject.GetComponent<NetworkAnimator>() != null)
            {
                //transform.parent.gameObject.AddComponent<RotationSync>();
                //transform.parent.gameObject.GetComponent<NetworkTransformChild>().target = transform;
                //transform.parent.gameObject.GetComponent<NetworkTransformChild>().enabled = true;
                transform.parent.gameObject.AddComponent<NetworkAnimator>().animator = TargetAnimator;
                GetComponent<NetworkIdentity>().enabled = false;
                //Destroy(GetComponent<NetworkIdentity>());
                active = false;
            }
        }
    }
}
