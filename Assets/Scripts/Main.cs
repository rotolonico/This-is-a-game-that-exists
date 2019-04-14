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
    public static bool Skip;
    public static bool isSkipMode;
    
    public Button SignTrigger;
    public GameObject GameStarter;
    public GameObject AdminCanvas;
    public GameObject AdminWindowTrigger;
    public GameObject AdminWindow;
    public GameObject AdminCheckMark;
    public Sprite AdminCheckMarkChecked;
    public InfoBox infoBox;
    public OptionBox optionBox;
    public SkipButton skipButton;

    private bool adminCheckMarkCheckedBool;

    private GameObject[] titleLetters;
    private GameObject[] letterHolders;
    public Image gameStarterImage;
    private Animator gameStarterAnimator;

    private void Start()
    {
        gameStarterImage = GameStarter.GetComponent<Image>();
        gameStarterAnimator = GameStarter.GetComponent<Animator>();
    }

    public void HoverNormalMode()
    {
        infoBox.PopupInfoBox("PLAY THE GAME NORMALLY", -1);
    }
    
    public void HoverSkipMode()
    {
        infoBox.PopupInfoBox("PLAY THE GAME BEING ABLE TO SKIP DIALOGUES", -1);
    }
    
    public void HoverExitNormalMode()
    {
        StartCoroutine(infoBox.PopdownInfoBox(0));
    }
    
    public void HoverExitSkipMode()
    {
        StartCoroutine(infoBox.PopdownInfoBox(0));
    }

    public void NormalMode()
    {
        isSkipMode = false;
        SceneManager.LoadScene(1);
    }
    
    public void SkipMode()
    {
        isSkipMode = true;
        SceneManager.LoadScene(1);
    }
    
    public void Initialize()
    {
        SoundHandler.sound.Play(SoundHandler.sound.a);
        StartCoroutine(AfterIntroduction());
        titleLetters = GameObject.FindGameObjectsWithTag("TitleLetter");
        letterHolders = GameObject.FindGameObjectsWithTag("LetterHolder");
    }

    private IEnumerator AfterIntroduction()
    {
        var t = SoundHandler.sound.a.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }

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
        var t = SoundHandler.sound.bba.length + 3;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
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
        var t = soundLength;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
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
        var t = sound.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(GameStillNotLoading());
    }

    private IEnumerator GameStillNotLoading()
    {
        SoundHandler.sound.Play(SoundHandler.sound.da);
        var t = SoundHandler.sound.da.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        AdminWindowTrigger.GetComponent<Animator>().Play("Opacity100");
        AdminWindowTrigger.GetComponent<Button>().interactable = true;
        yield return new WaitForSeconds(9.4f);
        if (adminCheckMarkCheckedBool) yield break;
        Destroy(AdminCanvas);
        SoundHandler.sound.Play(SoundHandler.sound.ea);
        var t2 = SoundHandler.sound.ea.length;
        if (isSkipMode) skipButton.PopupSkipButton(t2);
        while (t2 >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t2 = 1;
            }

            t2 -= Time.deltaTime;
            yield return null;
        }
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
        var t = SoundHandler.sound.eb.length - 9;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(GetToGirlGame());
    }

    private IEnumerator GetToGirlGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100P");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(2);
    }

    public IEnumerator StartGirlGame()
    {
        if (StoryHandler.fixedGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound.fa);
            var t = SoundHandler.sound.fa.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.fba);
            var t = SoundHandler.sound.fba.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
            
            if (StoryHandler.madeSignFall && StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbba);
                var t1 = SoundHandler.sound.fbba.length;
                if (isSkipMode) skipButton.PopupSkipButton(t1);
                while (t1 >= 0)
                {
                    if (Skip)
                    {
                        Skip = false;
                        t1 = 1;
                    }

                    t1 -= Time.deltaTime;
                    yield return null;
                }
            }
            if (StoryHandler.madeSignFall && !StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbbb);
                var t2 = SoundHandler.sound.fbbb.length;
                if (isSkipMode) skipButton.PopupSkipButton(t2);
                while (t2 >= 0)
                {
                    if (Skip)
                    {
                        Skip = false;
                        t2 = 1;
                    }

                    t2 -= Time.deltaTime;
                    yield return null;
                }
            }
            if (!StoryHandler.madeSignFall && StoryHandler.scrambledSign)
            {
                SoundHandler.sound.Play(SoundHandler.sound.fbbc);                
                var t3 = SoundHandler.sound.fbbc.length;
                if (isSkipMode) skipButton.PopupSkipButton(t3);
                while (t3 >= 0)
                {
                    if (Skip)
                    {
                        Skip = false;
                        t3 = 1;
                    }

                    t3 -= Time.deltaTime;
                    yield return null;
                }
            }
            
            SoundHandler.sound.Play(SoundHandler.sound.fbc);
            var t4 = SoundHandler.sound.fbc.length;
            if (isSkipMode) skipButton.PopupSkipButton(t4);
            while (t4 >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t4 = 1;
                }

                t4 -= Time.deltaTime;
                yield return null;
            }
        }
        
        infoBox.PopupInfoBox("YOU CAN NOW MOVE WITH \"WASD\"\nOR THE ARROW KEYS", 5);
        GPlayerController.activated = true;
    }

    public IEnumerator EndGirlGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.g);
        var t = SoundHandler.sound.g.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        
        optionBox.PopupOptionBox("WHAT DO YOU THINK OF THE GAME?", "IT'S GREAT", "IT'S KIND OF BUGGY");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }

        StartCoroutine(optionBox.playerChoice == 1 ? LikedGirlGame() : DislikedGirlGame());
        optionBox.playerChoice = 0;
    }

    private IEnumerator LikedGirlGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.ha);
        var t = SoundHandler.sound.ha.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
        
        if (StoryHandler.fixedGame)
        {
            StartCoroutine(FixedGame());
        }
        else
        {
            RandomGame();
        }
    }
    
    private IEnumerator DislikedGirlGame()
    {
        if (StoryHandler.fixedGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound.hba);
            var t = SoundHandler.sound.hba.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            StartCoroutine(FixedGame());
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.hbb);
            var t = SoundHandler.sound.hbb.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }

            RandomGame();
        }
    }

    private IEnumerator FixedGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.i);
        var t = SoundHandler.sound.i.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        
        optionBox.PopupOptionBox("DO YOU WANT TO GO WITH HIM?", "SURE", "NO, I'D RATHER STAY HERE");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }

        StartCoroutine(optionBox.playerChoice == 1 ? GetToRandomGame() : StayInGirlGame());
        StoryHandler.wentToGuyGame = optionBox.playerChoice == 1;

        optionBox.playerChoice = 0;
    }

    private IEnumerator StayInGirlGame()
    {
        if (StoryHandler.madeSignFall && StoryHandler.scrambledSign)
        {
            SoundHandler.sound.Play(SoundHandler.sound.jb);
            var t = SoundHandler.sound.jb.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.ja);
            var t = SoundHandler.sound.ja.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
        }
        
        yield return new WaitForSeconds(2f);
        RandomGame();
    }

    private void RandomGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.k);
        StartCoroutine(GetToRandomGame());
    }

    private IEnumerator GetToRandomGame()
    {
        SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100B");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(3);
    }
    
    public IEnumerator StartRandomGameLoading()
    {
        yield return new WaitForSeconds(1f);
        
        if (StoryHandler.wentToGuyGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound.la);
            var t = SoundHandler.sound.la.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.lb);
            var t = SoundHandler.sound.lb.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t = 1;
                }

                t -= Time.deltaTime;
                yield return null;
            }
        }

        SceneManager.LoadScene(4);
    }
    
    public IEnumerator StartRandomGame()
    {
        ClickHandler.Active = false;
        
        yield return new WaitForSeconds(1f);
        SoundHandler.sound.Play(SoundHandler.sound.m);
        var t = SoundHandler.sound.m.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
        
        optionBox.PopupOptionBox("WHAT DO YOU WANT TO DO?", "TELL HIM", "REMAIN SILENT");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }

        StoryHandler.saidName = optionBox.playerChoice == 1;
        optionBox.playerChoice = 0;
        
        if (StoryHandler.saidName)
        {
            SoundHandler.sound.Play(SoundHandler.sound.na);
            var t1 = SoundHandler.sound.na.length;
            if (isSkipMode) skipButton.PopupSkipButton(t);
            while (t1 >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t1 = 1;
                }

                t1 -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.nb);
            var t2 = SoundHandler.sound.nb.length;
            if (isSkipMode) skipButton.PopupSkipButton(t2);
            while (t2 >= 0)
            {
                if (Skip)
                {
                    Skip = false;
                    t2 = 1;
                }

                t2 -= Time.deltaTime;
                yield return null;
            }
        }
        
        SoundHandler.sound.Play(SoundHandler.sound.o);
        var t3 = SoundHandler.sound.o.length;
        if (isSkipMode) skipButton.PopupSkipButton(t3);
        while (t3 >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t3 = 1;
            }

            t3 -= Time.deltaTime;
            yield return null;
        }
        
        infoBox.PopupInfoBox("TRY DRAGGING THE BLOCKS TO THE LIGHT", 5);
        ClickHandler.Active = true;
    }

    public IEnumerator FinishFirstPuzzle()
    {
        SoundHandler.sound.Play(SoundHandler.sound.p);
        var t = SoundHandler.sound.p.length;
        if (isSkipMode) skipButton.PopupSkipButton(t);
        while (t >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t = 1;
            }

            t -= Time.deltaTime;
            yield return null;
        }
    }
    
    
}
