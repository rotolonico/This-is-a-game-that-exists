using UnityEngine;

namespace RandomGame
{
    public class RandomGameStarter : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().StartRandomGame());
        }
    }
}
