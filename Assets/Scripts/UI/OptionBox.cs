using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionBox : MonoBehaviour
    {
        public Text infoText;
        public Text option1Text;
        public Text option2Text;

        public int playerChoice;
    
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void PopupOptionBox(string info, string option1, string option2)
        {
            infoText.text = info;
            option1Text.text = option1;
            option2Text.text = option2;
        
            animator.Play("Popup");
        }

        public void Choose(int option)
        {
            playerChoice = option;
            animator.Play("Popdown");
        }
    }
}
