using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayersSpawner : NetworkBehaviour
{
    public GameObject BotPrefab;
    private NetworkGameManager gameManger;

    public void Awake()
    {
        gameManger = GameObject.FindObjectOfType<NetworkGameManager>();
    }

    public override void OnStartServer()
    {
        Vector3 pongOffset = new Vector3(0, 0, -1) * gameManger.ray;
        float angle = 360 / gameManger.pongNumber;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, angle));

        for (int i = 0; i < gameManger.pongNumber; i++)
        {
            GameObject bot = (GameObject)Instantiate(BotPrefab, pongOffset, Quaternion.Euler(new Vector3(0, angle * i)));
            bot.name = "Bot0" + i;
            pongOffset = rotation * pongOffset;
            NetworkServer.Spawn(bot);
        }
    }
}
