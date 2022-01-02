using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Enemy Wave Config")]  // важная строка: создаёт пункт в выпадающем на ПКМ меню
public class WaveConfig : ScriptableObject
{
    
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;


    public GameObject GetEnemyPrefab()  { return enemyPrefab; }

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>(); 
        foreach (Transform child in pathPrefab.transform)   // как работает foreach
        {                                               // 1. Обнуляет индекс компонентов (по логике) и "захватывает" указанное в ()
            waveWaypoints.Add(child);           // 2. выполняет код в {} ДЛЯ КАЖДОГО указанного в () объекта, в данном случае
        }                                   // он берёт дочерний объект префаба пути, и вносит Дочку префаба пути в список waveWaypoints 
        return waveWaypoints;       // По итогу, данный метод возвращает целиком список путевых точек! Но для того чтобы его вернуть,
    }                           // Данный метод создаёт перем-список waveWaypoints, и для каждый вейпойин из префаба помещает в список!

    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; } 
    // GetTimeBetweenSpawns метод возвращает перем. timeBetweenSpawns туда, где метод был вызван. это "get-метод" - "достань-метод",
    // он тащит инфу из кода в котором объявлен туда, где его вызвали. Перем. timeBetweenSpawns, очевидно, существует
    // в скрипте WaveConfig.cs однако, эта timeBetweenSpawns нужна другим скриптам. "get-метод" её доставляет как "транспорт". 

    public float GetSpawnRandomFactor() { return spawnRandomFactor; }

    public int GetNumberOfEnemies() { return numberOfEnemies; }

    public float GetMoveSpeed() { return moveSpeed; }


}
