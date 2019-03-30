using UnityEngine;

namespace GirlGame
{
    public class GirlGameStarter : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().StartGirlGame());
        }
    }
}
