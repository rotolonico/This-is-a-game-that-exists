using UnityEngine;

namespace SimonGame
{
    public class SimonGameStarter : MonoBehaviour
    {
        void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartSimonGame());
            mainHandler.gameStarterImage.enabled = false;
        }
    }
}
