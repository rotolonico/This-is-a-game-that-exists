using System.Collections;
using System.Collections.Generic;
using Handlers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button SignTrigger;
    
    public void Initialize()
    {
        SoundHandler.sound.Play(SoundHandler.sound.a);
        StartCoroutine(nameof(AfterIntroduction));
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
        foreach (var letter in GameObject.FindGameObjectsWithTag("TitleLetter"))
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
        foreach (var letterHolder in GameObject.FindGameObjectsWithTag("LetterHolder"))
        {
            letterHolder.GetComponent<LetterHolder>().active = true;
        }
    }

    public IEnumerator AfterSignRaised(bool combo)
    {
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
        yield return new WaitForSeconds(sound.length);
    }
}
