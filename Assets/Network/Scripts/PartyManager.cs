using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class PartyManager : NetworkBehaviour
{
    public static PartyManager instance;
    public PartyMember[] Party; // Party slots and team manager

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        //Party = new PartyMember[GameObject.Find("NetworkGameManager").GetComponent<NetworkGameManager>().pongNumber];
    }

    //[Command]
    //public void CmdAddPlayer() // Add player for Deathmatch
    //{
    //    for (int i = 0; i < Party.Length; i++)
    //        if (!Party[i].Slot) // Checks what slot index is free
    //        {
    //            AddPartyMember(i, true); // Adds the player to the party
    //                                     // send the index to the player
    //        }
    //}

    //[Command]
    //public void CmdAddPlayer(bool ATeam) // Add player for Team Deathmatch
    //{
    //    for (int i = 0; i < Party.Length; i++)
    //        if (!Party[i].Slot) // Checks what slot index is free
    //        {
    //            AddPartyMember(i, ATeam); // Adds the player to the party and put him in the currect team
    //                                      // send the index to the player
    //        }
    //}

    //[Command]
    //public void CmdRemovePlayer(int Index)
    //{
    //    Party[Index].Slot = false; // Tags the slot as free
    //}

    public void AddPartyMember(int Index, bool Ateam)
    {
        Party[Index].Slot = true; // Tags that the slot i is occupied
        Party[Index].ATeam = Ateam; // Tags if the player is in the ATeam or the BTeam
    }

    public void AddPartyMember(int Index)
    {
        Party[Index].Slot = true; // Tags that the slot i is occupied
        Party[Index].ATeam = true; // Tags if the player is in the ATeam
    }

    public class PartyMember
    {
        public bool Slot = false; // Party slot value (false if free, true if not)
        public bool ATeam = false; // Does it belong to the ATeam or the BTeam
    }

    public void OnServerConnect(NetworkConnection conn)
    {
        
    }

    public void OnServerDisconnect(NetworkConnection conn)
    {

    }
}
