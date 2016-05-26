using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetworkLobbyHook : LobbyHook
{
    // Used to pass variables from the lobby to the game
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NetworkLobbyInfo info = gamePlayer.GetComponent<NetworkLobbyInfo>();

        info.lobbyIndex = lobby.slot;
        Debug.Log("Lobby index = " + info.lobbyIndex);
        info.characterIndex = lobby.characterIndex;
        Debug.Log("Character index = " + info.characterIndex);
        info.playerName = lobby.playerName;
        Debug.Log("Name index = " + info.playerName);
    }
}
