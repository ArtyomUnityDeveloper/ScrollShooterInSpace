using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] float misileSpeed = 4f;

    [SerializeField] GameObject misileExplosion;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0, 1)] float soundVolume = 0.3f;

    [Header("Splash settings")]
    [SerializeField] bool splashMode = false;
    [SerializeField] float splashRange = 2.55f;



    // Update is called once per frame
    void Update()
    {
        Vector3 missilePos = transform.position;

        Vector3 missileVelocity = new Vector3(0, misileSpeed * Time.deltaTime, 0);

        missilePos += transform.rotation * missileVelocity;
        transform.position = missilePos;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            MissileBehaviour();
        }
        else if (other.tag == "GameBoundary")
        {
            Destroy(gameObject);
        }
    }

    private void MissileBehaviour()
    {
        if (splashMode)
        {
            var splashDamage = gameObject.GetComponent<DamageDealer>().GetDamage();
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);

            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.HitFromSplash(splashDamage);
                }
                Instantiate(misileExplosion, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, soundVolume);
            }
        }
        else
        {
            Destroy(gameObject);
            Instantiate(misileExplosion, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, soundVolume);
        }
    }
}