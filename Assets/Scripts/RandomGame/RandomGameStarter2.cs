using UnityEngine;

namespace RandomGame
{
    public class RandomGameStarter2 : MonoBehaviour
    {
        private void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartRandomGame2());
            mainHandler.gameStarterImage.enabled = false;
        }

        public void EndGame()
        {
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().FinishSecondPuzzle());
        }
    }
}
