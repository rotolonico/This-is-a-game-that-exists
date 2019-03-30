using System.Collections;
using System.Collections.Generic;
using GirlGame;
using Handlers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button SignTrigger;
    public GameObject GameStarter;
    public GameObject AdminCanvas;
    public GameObject AdminWindowTrigger;
    public GameObject AdminWindow;
    public GameObject AdminCheckMark;
    public Sprite AdminCheckMarkChecked;
    public InfoBox infoBox;
    public OptionBox optionBox;

    private bool adminCheckMarkCheckedBool;

    private GameObject[] titleLetters;
    private GameObject[] letterHolders;
    private Image gameStarterImage;
    private Animator gameStarterAnimator;
    
    public void Initialize()
    {
        SoundHandler.sound.Play(SoundHandler.sound.a);
        StartCoroutine(nameof(AfterIntroduction));
        titleLetters = GameObject.FindGameObjectsWithTag("TitleLetter");
        letterHolders = GameObject.FindGameObjectsWithTag("LetterHolder");
        gameStarterImage = GameStarter.GetComponent<Image>();
        gameStarterAnimator = GameStarter.GetComponent<Animator>();
    }

    private IEnumerator test()
    {
        optionBox.PopupOptionBox("Do you like me?", "Yes", "No");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }
        Debug.Log(optionBox.playerChoice);
        optionBox.playerChoice = 0;
    }

    private IEnumerator AfterIntroduction()
    {
        yield return new WaitForSeconds(SoundHandler.sound.a.length);
        infoBox.PopupInfoBox("THIS GAME HAS MULTIPLE ENDINGS\nEVERYTHING YOU DO WILL AFFECT THE STORYLINE", 5);
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
            StoryHandler.scrambledSign = true;
        } else if (!combo && !StoryHandler.madeSignFall)
        {
            sound = SoundHandler.sound.cb;
            StoryHandler.scrambledSign = true;
        }
        else
        {
            sound = SoundHandler.sound.cc;
            StoryHandler.scrambledSign = false;
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
        StoryHandler.fixedGame = true;
        SoundHandler.sound.Play(SoundHandler.sound.eb);
        yield return new WaitForSeconds(4);
        gameStarterAnimator.Play("Opacity100");
        yield return new WaitForSeconds(5);
        Destroy(AdminCanvas);
        gameStarterAnimator.enabled = false;
        gameStarterImage.enabled = false;
        yield return new WaitForSeconds(SoundHandler.sound.eb.length - 9);
        StartCoroutine(GetToGirlGame());
    }

    private IEnumerator GetToGirlGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100P");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(1);
    }

    public IEnumerator StartGirlGame()
    {
        if (StoryHandler.fixedGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound.fa);
            yield return new WaitForSeconds(SoundHandler.sound.fa.length);
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.fba);
            yield return new WaitForSeconds(SoundHandler.sound.fba.length);
            
            if (StoryHandler.madeSignFall && StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbba);
                yield return new WaitForSeconds(SoundHandler.sound.fbba.length);
            }
            if (StoryHandler.madeSignFall && !StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbbb);
                yield return new WaitForSeconds(SoundHandler.sound.fbbb.length);
            }
            if (!StoryHandler.madeSignFall && StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbbc);
                yield return new WaitForSeconds(SoundHandler.sound.fbbc.length);
            }
            
            SoundHandler.sound.Play(SoundHandler.sound.fbc);
            yield return new WaitForSeconds(SoundHandler.sound.fbc.length);
        }
        
        infoBox.PopupInfoBox("YOU CAN NOW MOVE WITH \"WASD\"\nOR THE ARROW KEYS", 5);
        GPlayerController.activated = true;
    }
}
