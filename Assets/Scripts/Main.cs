using System.Collections;
using System.Collections.Generic;
using Database;
using GirlGame;
using Handlers;
using SimonGame;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static bool Skip;
    public static bool GiveUp;
    public static bool isSkipMode;
    
    public GameObject GameStarter;
    public GameObject AdminCanvas;
    public GameObject AdminWindowTrigger;
    public GameObject AdminWindow;
    public GameObject AdminCheckMark;
    public Sprite AdminCheckMarkChecked;
    public InfoBox infoBox;
    public OptionBox optionBox;
    public SkipButton skipButtonScript;
    public GiveUpButton giveUpButtonScript;

    private bool adminCheckMarkCheckedBool;
    private bool onFirstPuzzle;

    private Button signTrigger;
    private GameObject[] titleLetters;
    private GameObject[] letterHolders;
    public Image gameStarterImage;
    public Animator gameStarterAnimator;
    private Button normalButton;
    private Button skipButton;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        normalButton = GameObject.Find("NormalMode").GetComponent<Button>();
        skipButton = GameObject.Find("SkipMode").GetComponent<Button>();
        gameStarterImage = GameStarter.GetComponent<Image>();
        gameStarterAnimator = GameStarter.GetComponent<Animator>();
    }

    private void Update()
    {
        if (GiveUp)
        {
            if (onFirstPuzzle)
            {
                StartCoroutine(FinishFirstPuzzle(true));
                onFirstPuzzle = false;
                GiveUp = false;
            }
        }
    }

    public void HoverNormalMode()
    {
        if (normalButton.interactable) infoBox.PopupInfoBox("PLAY THE GAME NORMALLY", -1);
    }
    
    public void HoverSkipMode()
    {
        if (skipButton.interactable) infoBox.PopupInfoBox("PLAY THE GAME BEING ABLE TO SKIP DIALOGUES", -1);
    }
    
    public void HoverExitNormalMode()
    {
        if (normalButton.interactable) StartCoroutine(infoBox.PopdownInfoBox(0));
    }
    
    public void HoverExitSkipMode()
    {
        if (skipButton.interactable) StartCoroutine(infoBox.PopdownInfoBox(0));
    }

    public void NormalMode()
    {
        isSkipMode = false;
        StartCoroutine(StartGame());
    }
    
    public void SkipMode()
    {
        isSkipMode = true;
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        DatabaseHandler.PostVariableToDatabase(1, "playedGame");
        normalButton.interactable = false;
        skipButton.interactable = false;
        SoundHandler.sound.Play(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(1);
    }
    
    public void Initialize()
    {
        StartCoroutine(AfterIntroduction());
        titleLetters = GameObject.FindGameObjectsWithTag("TitleLetter");
        letterHolders = GameObject.FindGameObjectsWithTag("LetterHolder");
        signTrigger = GameObject.Find("SignTrigger").GetComponent<Button>();
    }

    private IEnumerator AfterIntroduction()
    {
        yield return new WaitForSeconds(4);
        Destroy(GameObject.Find("Chapter"));
        
        SoundHandler.sound.Play(SoundHandler.sound.a);
        var t = SoundHandler.sound.a.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        signTrigger.interactable = true;
        StartCoroutine(nameof(AfterLoadingGame));
    }

    private IEnumerator AfterLoadingGame()
    {
        yield return new WaitForSeconds(10f);
        Destroy(signTrigger.gameObject);
        SoundHandler.sound.Play(SoundHandler.sound.bba);
        StartCoroutine(AfterTakingLonger());
    }
    
    private IEnumerator AfterTakingLonger()
    {
        var t = SoundHandler.sound.bba.length + 3;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            DatabaseHandler.PostVariableToDatabase(1, "madeSignFall");
            sound = SoundHandler.sound.ba;
            Destroy(signTrigger.gameObject);
        }
        else
        {
            sound = SoundHandler.sound.bbb;
            DatabaseHandler.PostVariableToDatabase(1, "nMadeSignFall");
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            DatabaseHandler.PostVariableToDatabase(1, "scrambledSign");
        } else if (!combo && !StoryHandler.madeSignFall)
        {
            sound = SoundHandler.sound.cb;
            StoryHandler.scrambledSign = true;
            DatabaseHandler.PostVariableToDatabase(1, "scrambledSign");
        }
        else
        {
            sound = SoundHandler.sound.cc;
            StoryHandler.scrambledSign = false;
            DatabaseHandler.PostVariableToDatabase(1, "nScrambledSign");
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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
        DatabaseHandler.PostVariableToDatabase(1, "nFixedGame");
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
        DatabaseHandler.PostVariableToDatabase(1, "fixedGame");
        SoundHandler.sound.Play(SoundHandler.sound.eb);
        yield return new WaitForSeconds(4);
        gameStarterAnimator.enabled = true;
        gameStarterImage.enabled = true;
        gameStarterAnimator.Play("Opacity100RE");
        yield return new WaitForSeconds(5);
        Destroy(AdminCanvas);
        gameStarterAnimator.enabled = false;
        gameStarterImage.enabled = false;        
        var t = SoundHandler.sound.eb.length - 9;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
                if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
                if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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
                if (isSkipMode) skipButtonScript.PopupSkipButton(t3);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t4);
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        StoryHandler.likedGirlGame = true;
        DatabaseHandler.PostVariableToDatabase(1, "likedGirlGame");
        SoundHandler.sound.Play(SoundHandler.sound.ha);
        var t = SoundHandler.sound.ha.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        DatabaseHandler.PostVariableToDatabase(1, "nLikedGirlGame");
        if (StoryHandler.fixedGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound.hba);
            var t = SoundHandler.sound.hba.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        if (optionBox.playerChoice == 1)
        {
            StoryHandler.wentToGuyGame = true;
            DatabaseHandler.PostVariableToDatabase(1, "wentToGuyGame");
        }
        else
        {
            DatabaseHandler.PostVariableToDatabase(1, "nWentToGuyGame");
        }
        

        optionBox.playerChoice = 0;
    }

    private IEnumerator StayInGirlGame()
    {
        if (StoryHandler.madeSignFall && StoryHandler.scrambledSign)
        {
            SoundHandler.sound.Play(SoundHandler.sound.jb);
            var t = SoundHandler.sound.jb.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        optionBox.playerChoice = 0;
        ClickHandler.Active = false;
        
        yield return new WaitForSeconds(1f);
        SoundHandler.sound.Play(SoundHandler.sound.m);
        var t = SoundHandler.sound.m.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            DatabaseHandler.PostVariableToDatabase(1, "saidName");
            SoundHandler.sound.Play(SoundHandler.sound.na);
            var t1 = SoundHandler.sound.na.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
            DatabaseHandler.PostVariableToDatabase(1, "nSaidName");
            SoundHandler.sound.Play(SoundHandler.sound.nb);
            var t2 = SoundHandler.sound.nb.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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
        if (isSkipMode) skipButtonScript.PopupSkipButton(t3);
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
        giveUpButtonScript.PopdownGiveUpButton();
        onFirstPuzzle = true;
        ClickHandler.Active = true;
    }

    public IEnumerator FinishFirstPuzzle(bool failed = false)
    {
        if (!failed) giveUpButtonScript.PopupGiveUpButton();
        foreach (var square in GameObject.FindGameObjectsWithTag("Square"))
        {
            square.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (failed)
        {
            StoryHandler.gaveUpFirstPuzzle = true;
            DatabaseHandler.PostVariableToDatabase(1, "gaveUpFirstPuzzle");
        }
        else
        {
            DatabaseHandler.PostVariableToDatabase(1, "nGaveUpFirstPuzzle");
        }
        SoundHandler.sound.Play(failed ? SoundHandler.sound.pa : SoundHandler.sound.p);
        var t = failed ? SoundHandler.sound.pa.length : SoundHandler.sound.p.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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

        var sound = SoundHandler.sound.qb;
        if (StoryHandler.likedGirlGame) sound = SoundHandler.sound.qa;
        
        SoundHandler.sound.Play(sound);
        var t1 = sound.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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


        if (StoryHandler.madeSignFall && StoryHandler.scrambledSign && !StoryHandler.wentToGuyGame)
        {
            sound = SoundHandler.sound.rb;
        }
        else
        {
            sound = SoundHandler.sound.ra;
        }
        
        SoundHandler.sound.Play(sound);
        var t2 = sound.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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

        infoBox.PopupInfoBox(StoryHandler.likedGirlGame ? "PAUL LEFT THE GAME" : "SERENA AND PAUL LEFT THE GAME",6);

        optionBox.playerChoice = 0;
            
        yield return new WaitForSeconds(2);
        SoundHandler.sound.Play(SoundHandler.sound.s);
        var t3 = SoundHandler.sound.s.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t3);
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
        
        optionBox.PopupOptionBox("WHAT DO YOU WANT TO DO?", "GO WITH SIMON", "STAY WITH DAVE");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }

        sound = optionBox.playerChoice == 1 ? SoundHandler.sound.ta : SoundHandler.sound.tb;
        optionBox.playerChoice = 0;
        
        SoundHandler.sound.Play(sound);
        var t4 = sound.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t4);
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

        sound = failed ? SoundHandler.sound.taa : SoundHandler.sound.tba;
        SoundHandler.sound.Play(sound);
        var t5 = sound.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t5);
        while (t5 >= 0)
        {
            if (Skip)
            {
                Skip = false;
                t5 = 1;
            }

            t5 -= Time.deltaTime;
            yield return null;
        }
        
        SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100BL");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(5);
    }

    public IEnumerator StartSimonGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.u);
        var t = SoundHandler.sound.v.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        
        infoBox.PopupInfoBox("YOU CAN NOW MOVE WITH \"WASD\"\nOR THE ARROW KEYS", 5);
        GameObject.Find("Player").GetComponent<SPlayerController>().isActive = true;

        if (!StoryHandler.likedGirlGame) yield break;
        SoundHandler.sound.Play(SoundHandler.sound.ua);
        var t1 = SoundHandler.sound.ua.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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

    public IEnumerator FinishSimonGame()
    {
        SoundHandler.sound.Play(SoundHandler.sound.v);
        var t = SoundHandler.sound.v.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        
        SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100B");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        mainCamera.transform.position = new Vector3(0, 0, -10);
        SceneManager.LoadScene(6);
    }

    public IEnumerator StartRandomGame2()
    {
        ClickHandler.Active = false;
        optionBox.playerChoice = 0;

        if (!StoryHandler.gaveUpFirstPuzzle)
        {
            SoundHandler.sound.Play(SoundHandler.sound.w);
            var t = SoundHandler.sound.w.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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

            optionBox.PopupOptionBox("WHAT DO YOU WANT TO SAY?", "I LIKED IT", "YOUR GAME IS BETTER");
            while (optionBox.playerChoice == 0)
            {
                yield return null;
            }

            if (optionBox.playerChoice == 1)
            {
                StoryHandler.likedSimonGame = true;
                DatabaseHandler.PostVariableToDatabase(1, "likedSimonGame");
                SoundHandler.sound.Play(SoundHandler.sound.xb);
                var t1 = SoundHandler.sound.xb.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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

                if (StoryHandler.likedGirlGame)
                {
                    SoundHandler.sound.Play(SoundHandler.sound.xba);
                    var t2 = SoundHandler.sound.ua.length;
                    if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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

                SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
                gameStarterImage.enabled = true;
                gameStarterAnimator.enabled = true;
                gameStarterAnimator.Play("Opacity100P");
                yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
                SceneManager.LoadScene(7);
            }
            else
            {
                DatabaseHandler.PostVariableToDatabase(1, "nLikedSimonGame");
                SoundHandler.sound.Play(SoundHandler.sound.xa);
                var t1 = SoundHandler.sound.xa.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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

                if (StoryHandler.likedGirlGame)
                {
                    SoundHandler.sound.Play(SoundHandler.sound.xaa);
                    var t2 = SoundHandler.sound.ua.length;
                    if (isSkipMode) skipButtonScript.PopupSkipButton(t2);
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

                ClickHandler.Active = true;
            }

            optionBox.playerChoice = 0;
        }
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.wa);
            var t = SoundHandler.sound.wa.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            
            if (StoryHandler.likedGirlGame)
            {
                SoundHandler.sound.Play(SoundHandler.sound.xba);
                var t1 = SoundHandler.sound.xba.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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

            SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
            gameStarterImage.enabled = true;
            gameStarterAnimator.enabled = true;
            gameStarterAnimator.Play("Opacity100P");
            yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
            SceneManager.LoadScene(7);
        }
    }

    public IEnumerator FinishSecondPuzzle()
    {
        SoundHandler.sound.Play(SoundHandler.sound.y);
        var t = SoundHandler.sound.y.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        
        SoundHandler.sound.PlaySecondary(SoundHandler.sound.loadingSound);
        gameStarterImage.enabled = true;
        gameStarterAnimator.enabled = true;
        gameStarterAnimator.Play("Opacity100P");
        yield return new WaitForSeconds(SoundHandler.sound.loadingSound.length);
        SceneManager.LoadScene(7);
    }

    public IEnumerator StartGirlGameEditor()
    {
        if (StoryHandler.likedGirlGame)
        {
            if (StoryHandler.gaveUpFirstPuzzle || StoryHandler.likedSimonGame)
            {
                SoundHandler.sound.Play(SoundHandler.sound.zc);
                var t = SoundHandler.sound.zc.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
                SoundHandler.sound.Play(SoundHandler.sound.zb);
                var t = SoundHandler.sound.zb.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        else
        {
            SoundHandler.sound.Play(SoundHandler.sound.za);
            var t = SoundHandler.sound.za.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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

        infoBox.PopupInfoBox("LEFT CLICK TO PLACE WALLS\nRIGHT CLICK TO REMOVE WALLS", 8);
        GameObject.Find("LeaveEditorButton").GetComponent<Button>().interactable = true;
        EditorHandler.activated = true;
        GPlayerController.activated = true;
        GPlayerController.editorMode = true;
    }

    public IEnumerator LeaveEditor()
    {
        StoryHandler.leftEditor = true;
        DatabaseHandler.PostVariableToDatabase(1, "leftEditor");
        EditorHandler.activated = false;
        GPlayerController.activated = false;
        GPlayerController.editorMode = false;
        
        SoundHandler.sound.Play(SoundHandler.sound._a);
        var t = SoundHandler.sound._a.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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

        Destroy(GameObject.Find("LeaveEditorButton"));
        
        SoundHandler.sound.Play(SoundHandler.sound._aa);
        var t1 = SoundHandler.sound._aa.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
        
        EditorHandler.activated = true;
        GPlayerController.activated = true;
        GPlayerController.editorMode = true;
    }

    public IEnumerator DeletePlayer()
    {
        Destroy(GameObject.Find("LeaveEditorButton"));
        StoryHandler.deletedPlayer = true;
        DatabaseHandler.PostVariableToDatabase(1, "deletedPlayer");
        SoundHandler.sound.Play(SoundHandler.sound._ba);
        var t = SoundHandler.sound._ba.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
        
        StartCoroutine(BigCrash());
    }

    public IEnumerator FinishEditor()
    {
        Destroy(GameObject.Find("LeaveEditorButton"));
        EditorHandler.activated = false;
        GPlayerController.activated = false;
        GPlayerController.editorMode = false;
        
        if (StoryHandler.leftEditor) DatabaseHandler.PostVariableToDatabase(1, "nLeftEditor");
        DatabaseHandler.PostVariableToDatabase(1, "nDeletedPlayer");
        
        if (StoryHandler.likedGirlGame)
        {
            SoundHandler.sound.Play(SoundHandler.sound._bc);
            yield return new WaitForSeconds(SoundHandler.sound._bc.length);
        }
        else
        {
            optionBox.playerChoice = 0;
            SoundHandler.sound.Play(SoundHandler.sound._bb);
            var t = SoundHandler.sound._bb.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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
            
            optionBox.PopupOptionBox("WHAT DO YOU THINK OF THE GAME NOW?", "IT'S GREAT", "IT'S STILL BAD");
            while (optionBox.playerChoice == 0)
            {
                yield return null;
            }

            if (optionBox.playerChoice == 1)
            {
                StoryHandler.likedGirlGameAgain = true;
                DatabaseHandler.PostVariableToDatabase(1, "likedGirlGameAgain");
                SoundHandler.sound.Play(SoundHandler.sound._ca);
                var t1 = SoundHandler.sound._ca.length;
                if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
                DatabaseHandler.PostVariableToDatabase(1, "nLikedGirlGameAgain");
                SoundHandler.sound.Play(SoundHandler.sound._cb);
                yield return SoundHandler.sound._cb.length;
            }
            optionBox.playerChoice = 0;
        }

        StartCoroutine(BigCrash());
    }

    private IEnumerator BigCrash()
    {
        SoundHandler.sound.Play(SoundHandler.sound._d);
        var t = SoundHandler.sound._d.length;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t);
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

        optionBox.playerChoice = 0;
        optionBox.PopupOptionBox("WHAT DO YOU WANT TO SAY?", "YES", "NO");
        while (optionBox.playerChoice == 0)
        {
            yield return null;
        }

        if (optionBox.playerChoice == 1)
        {
            StoryHandler.saidYes = true;
            DatabaseHandler.PostVariableToDatabase(1, "saidYes");
            
            SoundHandler.sound.Play(SoundHandler.sound._eb);
            var t1 = SoundHandler.sound._eb.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
            DatabaseHandler.PostVariableToDatabase(1, "nSaidYes");
            
            SoundHandler.sound.Play(SoundHandler.sound._ea);
            var t1 = SoundHandler.sound._ea.length;
            if (isSkipMode) skipButtonScript.PopupSkipButton(t1);
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
        
        DatabaseHandler.PostVariableToDatabase(1, isSkipMode ? "finishedChapterSkip" : "finishedChapterNormal");
        SceneManager.LoadScene(8);

        optionBox.playerChoice = 0;
    }

    public IEnumerator CreditsSkip()
    {
        var t = 60f;
        if (isSkipMode) skipButtonScript.PopupSkipButton(t, true);
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

        SceneManager.LoadScene(0);
    }
    
    
}
