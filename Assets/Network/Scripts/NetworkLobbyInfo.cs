using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyInfo : NetworkBehaviour
{
    [SyncVar]
    public int characterIndex;
    [SyncVar]
    public int lobbyIndex;
    [SyncVar]
    public string playerName;

    void Start()
    {
        gameObject.name = playerName;
    }

    // Just for debug
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkLobbyPlayer[] lobbyPlayers = GameObject.Find("LobbyManager").GetComponent<LobbyManager>().lobbySlots;

            for(int i = 0; i<lobbyPlayers.Length; i++)
            {
                if(lobbyPlayers[i] != null)
                    Debug.Log("Slot value = " + lobbyPlayers[i].slot);
                else
                    Debug.Log("Slot value = null");
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //NetworkLobbyInfo tmp = gameObject.GetComponent<NetworkLobbyInfo>();
            Debug.Log("CharacterIndex: " + characterIndex);
            Debug.Log("LobbyIndex: " + lobbyIndex);
            Debug.Log("PlayerName: " + playerName);
        }
    }
}
