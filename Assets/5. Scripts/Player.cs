using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float paddingSize = 0.2f; // по факту - отрезок от центра корабля игрока до его краёв
    [SerializeField] int health = 200; // hp игрока
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f; // [Range(0, 1)]  - бегунок от 0 до 1 - так проще настроить звук
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    [Header("Projectile")]
    public GameObject lazerPrefab;
    [SerializeField] int startLazerDamage = 20; // Не менять программно, оставлять заданное зн-е
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.5f; // сколько стреляет пушка после нажатия пробела

    [Header("Missile")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileDelay = 20f; //задержка между выстрелами
    [SerializeField] float nextShot; //время следующего выстрела
    [SerializeField] AudioClip missileLaunchSound;
    [SerializeField] [Range(0, 1)] float launchSoundVolume = 0.45f;

    string missileReadiness = "Missile ready!";


    Coroutine firingCoroutine; // "слот" под вспомогательную корутину
    DamageDealer myLaser; // тупа забиваем сюды нашу пуху, шоб било изи к ней обратиться

    int attackAdded = 0; // ВАЖНО! После смерти игрока ВЫЧЕСТЬ в DamageDealer лазера из damage данную переменную!

    float xMin, xMax, yMin, yMax;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        myLaser = lazerPrefab.GetComponent<DamageDealer>(); // кэширует компоненту DamageDealer - изменять урон лазера
        myLaser.SetDefaultDamage(startLazerDamage); // ВАЖНО! Вопрос решился этой строкой: всегда после смерти игрока урон лазера базовый
        firingCoroutine = StartCoroutine(FireContinuosly());
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        //Fire();
        MissileLaunch();
    }

    private void MissileLaunch()
    {
        if (Input.GetButton("Fire2") && Time.time > nextShot) // Time.time - The time at the beginning of this frame
        {
            Instantiate(missilePrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(missileLaunchSound, Camera.main.transform.position, launchSoundVolume);
            nextShot = Time.time + missileDelay;
        }
        MissileReady();
    }

    public string MissileReady()
    {
        if (Time.time > nextShot)
        {
            //Debug.Log("Missile ready!");
            missileReadiness = "Missile ready!";
        }
        else
        {
            //Debug.Log("Missile reloading...");
            missileReadiness = "Missile reloading...";
        }

        return missileReadiness;
    }

    IEnumerator FireContinuosly() // сделано чтобы стрельба велась БЕСКОНЕЧНО (в рамках данной сопрограммы)
    {
        while (true)        // таким образом цикл while зациклен!!! То есть: в его скобках всегда значение TRUE
        { 
        GameObject lazer = Instantiate(lazerPrefab,     // создана перем. равная инстанцированному префабу лазера
                                transform.position,     // появляется в позиции игрока
                                Quaternion.identity) as GameObject;     // является GameObject'ом
        lazer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);     // сразу же получает скорость перем-я

        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

        yield return new WaitForSeconds(projectileFiringPeriod);     // после заверш-я сопрограммы FireContinuosly идёт задержка
        }  // задержка нужна для того, чтобы регулировать скорость стрельбы игрока. При задержке 1 секунда скоростельность оч низкая
    }       // при задержке 0.05 секунды скорострельность огромная. По сути, задержка перед повторением цикла создающего префабы лазера
    

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        //Debug.Log(deltaX);
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin + paddingSize, xMax - paddingSize);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin + paddingSize, yMax - paddingSize);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return;  } 
        // имеется ввиду: если переменная damageDealer пустая, то прекрати выполнение метода сразу же на этой строке - return
        // т.о. при столкновении с объектом не имеющим DamageDealer.cs не выскочит NullReferenceException т.к. до выполнения
        // строки ProcessHit(damageDealer) программа не дойдёт. Человеческим языком: на данное столкновение не будет видимой реакции.
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver(); // тип - потому что Level.cs это и есть тип скрипта. Через точку обращ. к методу.
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        // т.к. у нас 2д игра - Camera.main.transform.position - играет звук прямо там где камера
        // иначе пришлось бы по дебильному двигать камеру ближе к врагам, что усложнило бы дальнейшую работу с игрой
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetShootDamage()
    {
        int damage = myLaser.GetDamage();
        return damage;
    }

    //public void SetDefaultDamage(int )

     public void AddWeaponDamage(int attackAdded)
     {
        //Debug.Log("Attack added: ");
        //Debug.Log(attackAdded);
        this.attackAdded = attackAdded;
        myLaser.AddWeaponDamage(attackAdded);
           //lazerDamage = myLaser.SetDamage(attackAdded);
     } 

    public void AddHealth(int healthRepaired)
    {
        //Debug.Log("Health added: ");
        //Debug.Log(healthRepaired);
        health += healthRepaired;
    }
}
