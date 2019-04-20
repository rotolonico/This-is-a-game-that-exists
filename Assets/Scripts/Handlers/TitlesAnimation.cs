using System.Collections;
using UnityEngine;

namespace Handlers
{
    public class TitlesAnimation : MonoBehaviour
    {
        public Animator titles;

        private void Start()
        {
            titles.Play("Titles");
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().CreditsSkip());
        }
    }
}
