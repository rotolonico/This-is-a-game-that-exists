using UnityEngine;

namespace GuyGame
{
    public class SignTrigger : MonoBehaviour
    {
        public void OnClick()
        {
            GameObject.Find("MainHandler").GetComponent<Main>().SignFalls(true);
        }
    }
}
