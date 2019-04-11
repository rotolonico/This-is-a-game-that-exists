using UnityEngine;

namespace GirlGame
{
    public class GirlGameStarter : MonoBehaviour
    {
        private void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartGirlGame());
            mainHandler.gameStarterImage.enabled = false;
        }
    }
}
