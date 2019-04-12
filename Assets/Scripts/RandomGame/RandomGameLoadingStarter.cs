using UnityEngine;

namespace RandomGame
{
    public class RandomGameLoadingStarter : MonoBehaviour
    {
        void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartRandomGameLoading());
            mainHandler.gameStarterImage.enabled = false;
            GameObject.Find("LoadingBar").GetComponent<Animator>().Play("Load");
        }
    }
}
