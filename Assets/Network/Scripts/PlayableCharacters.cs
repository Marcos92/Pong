using UnityEngine;
using System.Collections;

public class PlayableCharacters : MonoBehaviour
{
    public GameObject[] Characters;
    public GameObject[] Spawns;
    public static PlayableCharacters instance;

	void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
