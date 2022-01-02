using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] GameObject lootPrefab;
    [SerializeField] float minProbability = 0.3f;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] [Range(0, 1)] float spawnSoundVolume = 0.7f;
    float spawnProbability;


    public void LootSpawn()
    {
        spawnProbability = Random.Range(0f, 1f);
        //Debug.Log("Spawn Probability is: ");
        //Debug.Log(spawnProbability);

        if (spawnProbability > minProbability)
        {
            GameObject loot = Instantiate(lootPrefab, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position, spawnSoundVolume);
        }
        else
        {
            return;
        }

    }
}
