using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    Vector3 nextWaypoint;
    [SerializeField] float rotSpeed = 360f;
    [SerializeField] bool rotateTowardsPath = true;
    // Vector3 rotateWaypoint;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // двигает префаб противника каждый кадр
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        // переменная waveConfig класса EnemyPathing получает зн-е переменной waveConfig устанавливаемой методом SetWaveConfig
    }
    // SetWaveConfig - это "set-метод" - устанавливающий значение метод. В противоположность "get-методу", "set-метод" не
    // тащит переменную из скрипта в котором он объявлен в скрипт в котором вызван!!!
    // Наоборот!!! Данный метод тащит сюда, в EnemyPathing скрипт, значение типа WaveConfig переменной waveConfig
    // таким образом, EnemyPathing скрипт знает с каким WaveConfig ему работать в методе Start() - где он обращается
    // к указанному waveConfig с целью GetWaypoints() - взять get-методом Путевые Точки, а именно весь их список.



    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            nextWaypoint = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                                (transform.position, nextWaypoint, movementThisFrame);

            if (rotateTowardsPath)
            {
                RotationTowardsPathMethod();
            }

            /*   if (waypointIndex > 0)
               { 
                   rotateWaypoint = nextWaypoint; // передача точки индекс кот больш 1 - чтобы корабль вращался на неё
               } */


            if (transform.position == nextWaypoint)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RotationTowardsPathMethod()
    {
        Vector3 dir = waypoints[waypointIndex].transform.position - transform.position;
        dir.Normalize();
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
    // Если индекс ПутевойТочки меньше или равен количеству ПутьТочек-1 (если 0й вейпоинт, то его индекс меньше 5всего-1=4, делай усл.)
    // (индексы идут с нуля, поэтому минус 1 - для пятого вейпойнта будет инд. 4)., в общем!   :
    // при собл-ии усл-я делай: созданной в условии перем. targetPosition присвой поз-ю текущего вейпоинта(ПТ), т.о.
    // корабль врага идёт на нулевую ПТ. Далее: созданной далее перем. movement...Frame присвой скорость из waveConfig 
    // в которой состоит данный корабль помнож-ю на тайм.дельтаТ (чтобы на всех устройствах было планое перемещ-е)
    // трансформ.позишн данного объекта (кораблика) ебаш равной зн-ю метода MoveTowards (метод двигает точку к текущей цели)
    // ЕСЛИ позиция корабля врага == позиции цели, ТО увелич на единицу индекс вейпойнта(ПТ)
    // ИНАЧЕ уничтожить ИгровойОбъект (данный корабль врага) в связи с достижением ФИНАЛЬНОЙ ПУТЕВОЙ ТОЧКИ!


    /*public Vector3 GetNextWaypoint()
     {
         return rotateWaypoint;
     } */
}
