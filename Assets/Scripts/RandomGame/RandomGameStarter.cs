using UnityEngine;

namespace RandomGame
{
    public class RandomGameStarter : MonoBehaviour
    {
        void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartRandomGame());
            mainHandler.gameStarterImage.enabled = false;
            GameObject.Find("LoadingBar").GetComponent<Animator>().Play("Load");
        }
    }
}
