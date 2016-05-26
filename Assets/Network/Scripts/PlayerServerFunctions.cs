using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerServerFunctions : NetworkBehaviour
{
    void OnConnectedToServer()
    {
        Debug.Log("Connection successful");

        // Send message to the players
    }

    void OnDisconnectedFromServer()
    {
        Debug.Log("Disconnected");
        relocateSpawn();        

        // Send message to the players
    }

    public override void OnStartLocalPlayer()
    {
        // Add the chosen character to the player
    }

    void relocateSpawn()
    {
        PlayableCharacters.instance.Spawns[gameObject.GetComponent<NetworkLobbyInfo>().lobbyIndex].transform.position = gameObject.transform.position;
    }
}
