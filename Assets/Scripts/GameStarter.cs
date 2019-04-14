using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        var main = GameObject.Find("MainHandler").GetComponent<Main>();
        StartCoroutine(main.infoBox.PopdownInfoBox(0));
        main.Initialize();
    }
}
