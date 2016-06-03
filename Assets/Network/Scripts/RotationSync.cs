using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RotationSync : NetworkBehaviour 
{
    [SyncVar (hook = "Rotate")]
    public Quaternion playerRotation;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform.GetChild(0).transform;
        playerRotation = playerTransform.rotation;
    }

    //void Update()
    //{
    //    if (isServer)
    //    {
    //        if (playerRotation != playerTransform.rotation)
    //        {
    //            playerRotation = playerTransform.rotation;
    //            CmdTransmitRotations();
    //        }
    //    }
    //}

    void Update()
    {
        if (playerRotation != playerTransform.rotation)
        {
            playerRotation = playerTransform.rotation;
        }
    }

    void Rotate(Quaternion rotation)
    {
        playerTransform.rotation = rotation;
    }

    //[ClientRpc]
    //void RpcProvideRotationToServer(Quaternion playerRot)
    //{
    //    playerTransform.rotation = playerRot;
    //}

    ////[Command]
    //void CmdTransmitRotations()
    //{
    //    //if(isServer)
    //    //{
    //        RpcProvideRotationToServer(playerTransform.rotation);
    //    //}
    //}
}
