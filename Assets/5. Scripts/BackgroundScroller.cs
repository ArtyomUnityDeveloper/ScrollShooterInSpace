using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material myMaterial; // соединим КОД с МАТЕРИАЛОМ указаным в MeshRenderer. Класс Material описан в Unity ScriptingAPI.
    Vector2 offsetScroll; // вектор отвечающий за СМЕЩЕНИЕ (Offset - смещение)


    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material; // почему-то не MeshRenderer, а Renderer угловых скобках, но так надо
        offsetScroll = new Vector2(0, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offsetScroll * Time.deltaTime;
        //Debug.Log("Background scroll Upd TimeDeltaTime: " + Time.deltaTime);
        // ФреймРейтИндепендент прокрутка - т.к. умножаем на тайм.дельтаТайм
    }
}
