using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking;

public class NetworkCharacterOverride : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)

    {
        //OverrideCharacter();
        base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader); // Run the base OnServerAddPlayer
    }

    void OverrideCharacter(int index)
    {
        // Change the prefab value for the target one
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().playerPrefab = PlayableCharacters.instance.Characters[index];
    }
}
