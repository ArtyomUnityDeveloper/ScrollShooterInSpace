using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    private void Awake()
    {
        int musicPlayerCount = FindObjectsOfType<MusicPlayer>().Length;

        if (musicPlayerCount > 1)
        {
            gameObject.SetActive(false); // на случай бага
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
