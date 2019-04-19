using System.Collections;
using System.Collections.Generic;
using Handlers;
using UnityEngine;
using UnityEngine.UI;

public class GiveUpButton : MonoBehaviour
{
    private Button button;
    private Animator animator;

    private void Start()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    public void PopdownGiveUpButton()
    {
        button.interactable = true;
        animator.Play("Popdown2");
    }
        
    public void PopupGiveUpButton()
    {
        button.interactable = false;
        animator.Play("Popup2");
    }

    public void GiveUp()
    {
        PopupGiveUpButton();
        Main.GiveUp = true;
    }
}
