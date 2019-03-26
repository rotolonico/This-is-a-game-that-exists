using System.Collections;
using System.Collections.Generic;
using Handlers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button SignTrigger;
    public GameObject GameStarterImage;

    public GameObject AdminCanvas;
    public GameObject AdminWindowTrigger;
    public GameObject AdminWindow;
    public GameObject AdminCheckMark;
    public Sprite AdminCheckMarkChecked;

    private bool adminCheckMarkCheckedBool;

    private GameObject[] titleLetters;
    private GameObject[] letterHolders;
    
    public void Initialize()
    {
        SoundHandler.sound.Play(SoundHandler.sound.a);
        StartCoroutine(nameof(AfterIntroduction));
        titleLetters = GameObject.FindGameObjectsWithTag("TitleLetter");
        letterHolders = GameObject.FindGameObjectsWithTag("LetterHolder");
    }

    private IEnumerator AfterIntroduction()
    {
        yield return new WaitForSeconds(SoundHandler.sound.a.length);        
        SignTrigger.interactable = true;
        StartCoroutine(nameof(AfterLoadingGame));
    }

    private IEnumerator AfterLoadingGame()
    {
        yield return new WaitForSeconds(10f);
        Destroy(SignTrigger.gameObject);
        SoundHandler.sound.Play(SoundHandler.sound.bba);
        StartCoroutine(AfterTakingLonger());
    }
    
    private IEnumerator AfterTakingLonger()
    {
        yield return new WaitForSeconds(SoundHandler.sound.bba.length + 3);
        SignFalls(false);
    }

    public void SignFalls(bool playerFault)
    {
        AudioClip sound;
        if (playerFault)
        {
            StoryHandler.madeSignFall = true;
            sound = SoundHandler.sound.ba;
            Destroy(SignTrigger.gameObject);
        }
        else
        {
            sound = SoundHandler.sound.bbb;
        }

        ClickHandler.Active = true;
        foreach (var letter in titleLetters)
        {
            letter.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        
        SoundHandler.sound.Play(sound);
        
        StopCoroutine(AfterLoadingGame());
        StartCoroutine(AfterSignFalls(sound.length));
    }

    private IEnumerator AfterSignFalls(float soundLength)
    {
        yield return new WaitForSeconds(soundLength);
        foreach (var letterHolder in letterHolders)
        {
            letterHolder.GetComponent<LetterHolder>().active = true;
        }
    }

    public IEnumerator AfterSignRaised(bool combo)
    {
        foreach (var letter in titleLetters)
        {
            Destroy(letter.GetComponent<Draggable>());
        }
        AudioClip sound;

        if (!combo && StoryHandler.madeSignFall)
        {
            sound = SoundHandler.sound.ca;
        } else if (!combo && !StoryHandler.madeSignFall)
        {
            sound = SoundHandler.sound.cb;
        }
        else
        {
            sound = SoundHandler.sound.cc;
        }
        
        SoundHandler.sound.Play(sound);
        if (!combo && !StoryHandler.madeSignFall)
        {
            yield return new WaitForSeconds(3.5f);
            foreach (var letter in titleLetters)
            {
                foreach (var letterHolder in letterHolders)
                {
                    if (letterHolder.name[0] == letter.name[0])
                    {
                        letter.transform.position = letterHolder.transform.position;
                    }
                }
            }
        }
        yield return new WaitForSeconds(sound.length + 3);
        StartCoroutine(GameStillNotLoading());
    }

    private IEnumerator GameStillNotLoading()
    {
        SoundHandler.sound.Play(SoundHandler.sound.da);
        yield return new WaitForSeconds(SoundHandler.sound.da.length);
        AdminWindowTrigger.GetComponent<Animator>().Play("Opacity100");
        AdminWindowTrigger.GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(9.4f);
        if (adminCheckMarkCheckedBool) yield break;
        Destroy(AdminCanvas);
        SoundHandler.sound.Play(SoundHandler.sound.ea);
        yield return new WaitForSeconds(SoundHandler.sound.ea.length);
        StartCoroutine(GetToGirlGame());
    }

    public void OpenAdminWindow()
    {
        AdminWindowTrigger.GetComponent<Animator>().Play("Popup");
        AdminWindow.GetComponent<Animator>().Play("Popup");
    }

    public void LoadGame()
    {
        StartCoroutine(TryToLoadGame());
    }

    private IEnumerator TryToLoadGame()
    {
        if (adminCheckMarkCheckedBool) yield break;
        adminCheckMarkCheckedBool = true;
        AdminCheckMark.GetComponent<Image>().sprite = AdminCheckMarkChecked;
        SoundHandler.sound.Play(SoundHandler.sound.eb);
        yield return new WaitForSeconds(4f);
        GameStarterImage.GetComponent<Animator>().Play("Opacity100");
        yield return new WaitForSeconds(5f);
        GameStarterImage.GetComponent<Image>().enabled = false;
    }

    private IEnumerator GetToGirlGame()
    {
        yield return new WaitForSeconds(3);
    }
}
