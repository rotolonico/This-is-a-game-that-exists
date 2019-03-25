using System.Collections;
using System.Collections.Generic;
using Handlers;
using UnityEngine;

public class LetterHolder : MonoBehaviour
{
    public bool filled;
    public char letter;

    public bool active;

    private Rigidbody2D letterRb;
    private GameObject[] letterHolders;

    private void Start()
    {
        letterHolders = GameObject.FindGameObjectsWithTag("LetterHolder");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!active || filled || !other.gameObject.CompareTag("TitleLetter") || other.gameObject.GetComponent<Rigidbody2D>() == ClickHandler.DraggedColliderRb) return;
        SoundHandler.sound.Play(SoundHandler.sound.a);
        letterRb = other.gameObject.GetComponent<Rigidbody2D>();
        filled = true;
        var otherTransform = other.transform;
        otherTransform.position = transform.position;
        otherTransform.rotation = transform.rotation;
        other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        letter = other.name[0];
        CheckHolders();
    }

    private void Update()
    {
        if (filled && Input.GetMouseButton(0) && ClickHandler.DraggedColliderRb == letterRb)
        {
            filled = false;
            letterRb = null;
        }
    }

    private void CheckHolders()
    {
        var correct = true;
        foreach (var letterHolder in letterHolders)
        {
            if (letterHolder.GetComponent<LetterHolder>().filled == false) return;
            if (letterHolder.GetComponent<LetterHolder>().letter != letterHolder.name[0])
            {
                correct = false;
            }
        }

        StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().AfterSignRaised(correct));
    }
}
