using System.Collections;
using Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkipButton : MonoBehaviour
    {
        private Button button;
        private Animator animator;

        private void Start()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
        }

        public void PopupSkipButton(float time)
        {
            animator.Play("Popup");
            StartCoroutine(PopdownSkipButton(time));
        }
        
        private IEnumerator PopdownSkipButton(float time)
        {
            if (time > 5) time = 5;
            yield return new WaitForSeconds(1);
            button.interactable = true;
            yield return new WaitForSeconds(time -1.5f);
            animator.Play("Popdown");
            button.interactable = false;
        }

        public void Skip()
        {
            animator.Play("Popdown");
            button.interactable = false;
            SoundHandler.sound.Stop();
            SoundHandler.sound.StopSecondary();
            Main.Skip = true;
            StopAllCoroutines();
        }

    }
}
