using UnityEngine;

namespace GirlGame
{
    public class GirlGameEditorStarter : MonoBehaviour
    {
        void Start()
        {
            var mainHandler = GameObject.Find("MainHandler").GetComponent<Main>();
            StartCoroutine(mainHandler.StartGirlGameEditor());
            mainHandler.gameStarterImage.enabled = false;
            mainHandler.gameStarterAnimator.enabled = false;
        }
    }
}
