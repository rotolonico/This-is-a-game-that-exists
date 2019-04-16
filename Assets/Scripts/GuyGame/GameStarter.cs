using UnityEngine;

namespace GuyGame
{
    public class GameStarter : MonoBehaviour
    {
        private void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.infoBox.PopdownInfoBox(0));
            mainHandler.gameStarterImage.enabled = false;
            mainHandler.gameStarterAnimator.enabled = false;
            mainHandler.Initialize();
        }
    }
}
