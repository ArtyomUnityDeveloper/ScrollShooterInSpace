using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehavior : MonoBehaviour
{
    [Header("Loot Movement")]
    [SerializeField] float lootVelocity = 1f;

    [Header("Attack")]
    [SerializeField] bool attackAmpl = false;
    [SerializeField] int attackAdded = 5;
    [SerializeField] AudioClip attackBoostSound;
    [SerializeField] [Range(0, 1)] float attackBoostVolume = 0.7f;

    [Header("Repair")]
    [SerializeField] bool repairShip = false;
    [SerializeField] int healthRepaired = 100;
    [SerializeField] AudioClip repairBoostSound;
    [SerializeField] [Range(0, 1)] float repairBoostVolume = 0.7f;

    [Header("Score")]
    [SerializeField] bool ptsLoot = false;
    [SerializeField] int scoreGained = 250;
    [SerializeField] AudioClip scoreBoostSound;
    [SerializeField] [Range(0, 1)] float scoreBoostVolume = 0.7f;

    Player player; // через это вызываем метод в игроке
    GameSession gameSession;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lootPosition = transform.position;
        Vector2 lootMovement = new Vector2(0, lootVelocity * Time.deltaTime);
        lootPosition -= lootMovement;
        transform.position = lootPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PowerUpOptions();
        }
        else if (other.tag == "GameBoundary")
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("3rd condition destruction");
            Destroy(gameObject);
        }
    }

    private void PowerUpOptions()
    {
        if (attackAmpl)
        {
            //Debug.Log("Attack boost");
            AudioSource.PlayClipAtPoint(attackBoostSound, Camera.main.transform.position, attackBoostVolume);
            Destroy(gameObject);
            player.AddWeaponDamage(attackAdded);
        }
        else if (repairShip)
        {
            //Debug.Log("Repair boost");
            AudioSource.PlayClipAtPoint(repairBoostSound, Camera.main.transform.position, repairBoostVolume);
            Destroy(gameObject);
            player.AddHealth(healthRepaired);
        }
        else if (ptsLoot)
        {
            //Debug.Log("Pts boost");
            AudioSource.PlayClipAtPoint(scoreBoostSound, Camera.main.transform.position, scoreBoostVolume);
            Destroy(gameObject);
            gameSession.AddToScore(scoreGained);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
