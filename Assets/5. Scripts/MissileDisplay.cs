using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    Text missileText;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        missileText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        missileText.text = player.MissileReady();
    }
}
