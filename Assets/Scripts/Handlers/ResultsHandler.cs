using System;
using Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Handlers
{
    public class ResultsHandler : MonoBehaviour
    {
        public RectTransform madeSignFallF;
        public RectTransform scrambledSignF;
        public RectTransform fixedGameF;
        public RectTransform likedGirlGameF;
        public RectTransform wentToGuyGameF;
        public RectTransform saidNameF;
        public RectTransform likedSimonGameF;
        public RectTransform gaveUpFirstPuzzleF;
        public RectTransform leftEditorF;
        public RectTransform deletedPlayerF;
        public RectTransform likedGirlGameAgainF;
        public RectTransform saidYesF;
        
        public Text madeSignFallP;
        public Text scrambledSignP;
        public Text fixedGameP;
        public Text likedGirlGameP;
        public Text wentToGuyGameP;
        public Text saidNameP;
        public Text likedSimonGameP;
        public Text gaveUpFirstPuzzleP;
        public Text leftEditorP;
        public Text deletedPlayerP;
        public Text likedGirlGameAgainP;
        public Text saidYesP;

        public Text played;
        public Text finishedNormal;
        public Text finishedSkip;

        private void Start()
        {
            DatabaseHandler.GetChapter1FromDatabase(chapter =>
            {
                Debug.Log(chapter.nMadeSignFall);
                var madeSignFall = GetPercentage(chapter.madeSignFall, chapter.nMadeSignFall);
                var scrambledSign = GetPercentage(chapter.scrambledSign, chapter.nScrambledSign);
                var fixedGame = GetPercentage(chapter.fixedGame, chapter.nFixedGame);
                var likedGirlGame = GetPercentage(chapter.likedGirlGame, chapter.nLikedGirlGame);
                var wentToGuyGame = GetPercentage(chapter.wentToGuyGame, chapter.nWentToGuyGame);
                var saidName = GetPercentage(chapter.saidName, chapter.nSaidName);
                var likedSimonGame = GetPercentage(chapter.likedSimonGame, chapter.nLikedSimonGame);
                var gaveUpFirstPuzzle = GetPercentage(chapter.gaveUpFirstPuzzle, chapter.nGaveUpFirstPuzzle);
                var leftEditor = GetPercentage(chapter.leftEditor, chapter.nLeftEditor);
                var deletedPlayer = GetPercentage(chapter.deletedPlayer, chapter.nDeletedPlayer);
                var likedGirlGameAgain = GetPercentage(chapter.likedGirlGameAgain, chapter.nLikedGirlGameAgain);
                var saidYes = GetPercentage(chapter.saidYes, chapter.nSaidYes);
                
                Debug.Log(madeSignFallF.position);
                madeSignFallF.position += new Vector3(madeSignFall*5, 0, 0);
                scrambledSignF.position += new Vector3(scrambledSign*5, 0, 0);
                fixedGameF.position += new Vector3(fixedGame*5, 0, 0);
                likedGirlGameF.position += new Vector3(likedGirlGame*5, 0, 0);
                wentToGuyGameF.position += new Vector3(wentToGuyGame*5, 0, 0);
                saidNameF.position += new Vector3(saidName*5, 0, 0);
                likedSimonGameF.position += new Vector3(likedSimonGame*5, 0, 0);
                gaveUpFirstPuzzleF.position += new Vector3(gaveUpFirstPuzzle*5, 0, 0);
                leftEditorF.position += new Vector3(leftEditor*5, 0, 0);
                deletedPlayerF.position += new Vector3(deletedPlayer*5, 0, 0);
                likedGirlGameAgainF.position += new Vector3(likedGirlGameAgain*5, 0, 0);
                saidYesF.position += new Vector3(saidYes*5, 0, 0);

                madeSignFallP.text = madeSignFall + "%";
                scrambledSignP.text = scrambledSign + "%";
                fixedGameP.text = fixedGame + "%";
                likedGirlGameP.text = likedGirlGame + "%";
                wentToGuyGameP.text = wentToGuyGame + "%";
                saidNameP.text = saidName + "%";
                likedSimonGameP.text = likedSimonGame + "%";
                gaveUpFirstPuzzleP.text = gaveUpFirstPuzzle + "%";
                leftEditorP.text = leftEditor + "%";
                deletedPlayerP.text = deletedPlayer + "%";
                likedGirlGameAgainP.text = likedGirlGameAgain + "%";
                saidYesP.text = saidYes + "%";

                played.text = chapter.playedGame + " PLAYERS PLAYED THIS CHAPTER";
                finishedNormal.text = chapter.finishedChapterNormal + " PLAYERS FINISHED THIS CHAPTER\nIN NORMAL MODE";
                finishedSkip.text = chapter.finishedChapterSkip + " PLAYERS FINISHED THIS CHAPTER\nIN SKIP MODE";

            }, () => { SceneManager.LoadScene(0); });
        }

        private int GetPercentage(float variable, float nVariable)
        {
            if (Math.Abs(variable + nVariable) < 0.5f) return 50;
            Debug.Log(variable / (variable + nVariable) * 100f);
            return (int)(variable / (variable + nVariable) * 100f);
        }
    }
}
