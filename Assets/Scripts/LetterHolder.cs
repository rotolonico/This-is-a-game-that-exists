using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        char[] letters = new char[4];
        for (var i = 0; i < letterHolders.Length; i++)
        {
            var letterHolder = letterHolders[i];
            if (letterHolder.GetComponent<LetterHolder>().filled == false) return;
            letters[i] = letterHolder.GetComponent<LetterHolder>().letter;
            if (letterHolder.GetComponent<LetterHolder>().letter != letterHolder.name[0])
            {
                correct = false;
            }
        }

        if (letters.Where((letter, i) => letters.Where((t, j) => i != j).Any(letter2 => letter == letter2)).Any())
        {
            return;
        }

        StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().AfterSignRaised(correct));
    }
}
