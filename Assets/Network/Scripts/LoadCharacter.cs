using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadCharacter : NetworkBehaviour
{
    bool attached = false;
    string childName;


    void Start()
    {
        NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();

        childName = PlayableCharacters.instance.Characters[infoTmp.characterIndex].name;
        if (hasAuthority)
        {
            CmdLoad();
        }
    }


    [ClientCallback]
    void Update()
    {
        if (!attached)
        {
            GameObject filho = GameObject.Find(childName + "(Clone)");

            if (filho != null)
            {
                Debug.Log("so far so good!");

                


                filho.transform.parent = transform;
                GetComponent<NetworkTransformChild>().target = filho.transform;

                attached = true;

                if (isServer)
                {
                    filho.transform.rotation = transform.rotation;
                    filho.transform.position = transform.position;
                }
                else
                {
                    NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();
                    GameObject spawnTmp = PlayableCharacters.instance.Spawns[infoTmp.lobbyIndex];

                    transform.rotation = spawnTmp.transform.rotation;
                    transform.position = spawnTmp.transform.position;
                }

                



                Transform placeHolder = transform.FindChild("Placeholder");
                if (placeHolder != null)
                {
                    placeHolder.parent = null;


                    //				transform.rotation = placeHolder.rotation;
                    //				transform.position = placeHolder.position;
                    //
                    Destroy(placeHolder.gameObject);
                }
            }
        }
    }



    [ClientRpc]
    public void RpcSetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        for (int i = 0; i < transform.childCount; i++)
        { // just in case;
            transform.GetChild(i).rotation = rotation;
        }
        Debug.Log("Called rotation set!");
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        transform.position = position;
        Debug.Log("Called position set!");
    }
    [ClientRpc]
    public void RpcSetChild(string s)
    {
        //	ChildName = s;
    }

    [Command]
    public void CmdLoad()
    {
        NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();
        GameObject spawnTmp = PlayableCharacters.instance.Spawns[infoTmp.lobbyIndex];

        transform.position = spawnTmp.transform.position;
        transform.rotation = spawnTmp.transform.rotation;

        GameObject character = (GameObject)Instantiate(PlayableCharacters.instance.Characters[infoTmp.characterIndex],
                                   Vector3.zero, Quaternion.identity);
        //   spawnTmp.transform.position, spawnTmp.transform.rotation);
        //		character.name = PlayableCharacters.instance.Characters [infoTmp.characterIndex].name;

        character.transform.parent = gameObject.transform;
        //		character.transform.position = transform.position;
        //		character.transform.rotation = transform.rotation;

        //	RpcSetChild (character.name);
        //	RpcSetRotation(transform.rotation);
        //	RpcSetPosition (transform.position);


        //GetComponent<NetworkTransform> ().enabled = false;
        GetComponent<NetworkTransformChild>().target = character.transform;

        Debug.Log("Destroying placeholder at " + gameObject.name);
        Transform placeholder = transform.FindChild("Placeholder");
        placeholder.parent = null;
        Destroy(placeholder.gameObject);

        NetworkServer.Spawn(character);
    }
}
