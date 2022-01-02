using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;


    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }


    public void AddWeaponDamage(int attackAdded)
    {
        //Debug.Log("Attack added TO PLAYER'S LAZER: ");
        //Debug.Log(attackAdded);
        damage += attackAdded;
        
           //lazerDamage = myLaser.SetDamage(attackAdded);
     }

    public void SetDefaultDamage(int defaultDamage)
    {
        damage = defaultDamage;
    }
}
