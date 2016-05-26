using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GiveTransform : NetworkBehaviour
{
    public NetworkTransformChild TransformChild;

	void Awake()
    {
        TransformChild.target = transform.GetChild(0).transform;
    }
}
