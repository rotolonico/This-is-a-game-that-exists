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
        SoundHandler.sound.Play(SoundHandler.sound.bbb);
    }

    public void SignFalls(bool playerFault)
    {
        if (playerFault)
        {
            StoryHandler.madeSignFall = true;
            SoundHandler.sound.Play(SoundHandler.sound.ba);
            Destroy(SignTrigger.gameObject);
        }

        ClickHandler.Active = true;
        foreach (var letter in GameObject.FindGameObjectsWithTag("TitleLetter"))
        {
            letter.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        StopCoroutine(nameof(AfterLoadingGame));
    }
}
