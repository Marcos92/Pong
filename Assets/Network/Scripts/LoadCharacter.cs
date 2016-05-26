using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadCharacter : NetworkBehaviour
{
    void Start()
    {
        CmdLoad();
    }
    
    [Command]
    public void CmdLoad()
    {
        NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();
        GameObject spawnTmp = PlayableCharacters.instance.Spawns[infoTmp.lobbyIndex];
        gameObject.transform.position = spawnTmp.transform.position;
        gameObject.transform.rotation = spawnTmp.transform.rotation;
        GameObject character = (GameObject)Instantiate(PlayableCharacters.instance.Characters[infoTmp.characterIndex], spawnTmp.transform.position, spawnTmp.transform.rotation);
        //character.transform.position = Vector3.zero;
        //character.transform.rotation = Quaternion.identity;
        Transform placeholder = transform.FindChild("Placeholder");
        placeholder.parent = null;
        Destroy(placeholder.gameObject);
        character.transform.parent = gameObject.transform;
        gameObject.GetComponent<NetworkTransformChild>().target = character.transform;
        NetworkServer.Spawn(character);
    }
}
