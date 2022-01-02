using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    Text damageText;
    Player player;


    // Скрипт применяется для того, чтобы выводить на экран текущий урон с одного лазерного выстрела
    // По ходу игры игрок набирает усиления осн. орудия, дисплей к которому привязан скрипт
    // помогает наблюдать прогресс

    void Start()
    {
        damageText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    
    void Update()
    {
        damageText.text = player.GetShootDamage().ToString();
    }
}
