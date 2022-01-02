//using System;
using System;
/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class EnemyFlyingTank : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;


    [Header("Shooting")]
    float shotCounter; // счётчик выстрелов, через него, видимо, считать будем время до сл. выстрела
    [SerializeField] float minTimeBetweenShots = 0.2f; // для рандомизации стрельбы
    [SerializeField] float maxTimeBetweenShots = 3f; // тож для рандом стрельбы
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    Transform playerCoordinates; // тут наш летающий танк будет хранить параметр координат игрока
    [SerializeField] float rotationSpeed = 360f;  // скорость поворота "танка" при наведении на игрока
    GameObject playerCaptured; // захват игрока в "систему наведения" - чтоб "танк" поворачивался на него


    [Header("Enemy VFX and SFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f; // [Range(0, 1)]  - бегунок от 0 до 1 - так проще настроить звук
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    [Header("Hit VFX and SFX")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] float durationOfHit = 1;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0, 1)] float hitSoundVolume = 0.4f; // [Range(0, 1)]  - бегунок от 0 до 1 - так проще настроить звук


    //EnemyPathing enemyPathing;
    //private Rigidbody2D rb;
    //private float rotZ;



    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        playerCaptured = GameObject.Find("Player");

        //enemyPathing = FindObjectOfType<EnemyPathing>();
        //rb = this.GetComponent<Rigidbody2D>();  // всадили в гнездо наш скриптец
        // преф. врага инстанцирован -> сразу рандомим зн-е счётчика до сл. выстрела
    }

    // Update is called once per frame
    void Update()
    {
        SearchForPlayer();
        TurnTowardsPlayer();
        CountDownAndShoot(); // веди отсчёт и стреляй. по факту: перезапуск счётчика и стрельба
        //Rotate();
    }

    private void TurnTowardsPlayer()
    {
        Vector3 dir = playerCoordinates.position - transform.position; // это вектор направления на игрока
        dir.Normalize(); // нормализ-й вект. - тоже направление, длина вект. = 1
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90; // угол поворота получен этой формулой 

        Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle); // перевод угла в кватернион
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime); //наведи курсор на RotateTowards - всё очевидно и просто

    }

    private void SearchForPlayer()
    {
        if (playerCaptured == null)
        {
            playerCaptured = GameObject.Find("Player");
        }
        else
        {
            playerCoordinates = playerCaptured.transform;
        }
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime; // отсчёт до выстрелов FRAME RATE INDEPENDENT благодаря исп-ю Time.deltaTime
// по сути это: shotCounter = shotCounter - ВРЕМЯкот.ЗанимаетВыполнениеОдногоКадра кажд. кадр. Когда shotCounter = 0 происх. выстрел
        if (shotCounter <= 0f) // shotCounter дошёл до нуля тогда делай выстрел
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        Vector3 offset1 = transform.rotation * new Vector3(0.1f, 0, 0);
        Vector3 offset2 = transform.rotation * new Vector3(-0.1f, 0, 0);

        GameObject launch1 = Instantiate(projectile,     // создана перем. равная инстанцированному префабу ракеты
                   transform.position + offset1,     // появляется в позиции игрока
                   transform.rotation) as GameObject;     // является GameObject'ом
        GameObject launch2 = Instantiate(projectile,     // создана перем. равная инстанцированному префабу ракеты
           transform.position + offset2,     // появляется в позиции игрока
           transform.rotation) as GameObject;     // является GameObject'ом

        //laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);     // отриц скорость снаряда дабы 
        // летели вниз, туда где игрок    Quaternion.AngleAxis(180,new Vector3(0, 0, 1) - чтобы развернуть на 180 по оси Z

        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

        /*
        https://freesound.org/people/smcameron/sounds/51468/ 
        юзаю звук missile_launch_2.wav by smcameron
         */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject explosionHit = Instantiate(hitVFX, transform.position, transform.rotation);
        Destroy(explosionHit, durationOfHit);
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);

        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

  /*  private void Rotate()
    {
        Vector3 targetPosition = new Vector3(enemyPathing.GetNextWaypoint().x,
                                            enemyPathing.GetNextWaypoint().y,
                                            enemyPathing.GetNextWaypoint().z);
        transform.LookAt(targetPosition);



         // Determine which direction to rotate towards
        Vector3 targetDirection = enemyPathing.GetNextWaypoint() - transform.position;
        //Debug.Log("Target direction is = " + targetDirection); // www. youtube. com/watch?v=4Wh22ynlLyk полезный видосик
        float angle = Mathf.Atan2(targetDirection.x, targetDirection.y) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;  // + 124  + 90  

           // The step size is equal to speed times frame time.
            float singleStep = angularRotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection); 
    } */
}