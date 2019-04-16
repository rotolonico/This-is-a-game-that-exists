using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoBox : MonoBehaviour
    {
        private Text infoText;
        private Animator animator;

        private void Start()
        {
            infoText = GetComponentInChildren<Text>();
            animator = GetComponent<Animator>();
        }

        public void PopupInfoBox(string text, float time)
        {
            infoText.text = text;
            animator.Play("Popup");
            if (time > 0) StartCoroutine(PopdownInfoBox(time));
        }

        public IEnumerator PopdownInfoBox(float time)
        {
            yield return new WaitForSeconds(time);
            animator.Play("Popdown");
        }
    }
}
