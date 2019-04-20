using Proyecto26;
using UnityEngine;

namespace Database
{
    public class DatabaseHandler : MonoBehaviour
    {
        public static string ProjectId = "unfriendlybot-105be";

        public static string DatabaseUrl = "https://" + ProjectId + ".firebaseio.com/";
        
        public delegate void GetChapterCallback(Chapter1 chapter);
        public delegate void GetChapterFallback();

        public static void PostVariableToDatabase(int chapterNumber, string variable)
        {
            RestClient.Get(DatabaseUrl + "Chapter" + chapterNumber + "/" + variable + ".json").Then(value =>
            {
                var payLoad = "\"" + (int.Parse(value.Text.Trim('"')) + 1) + "\"";
                RestClient.Put(DatabaseUrl + "Chapter" + chapterNumber + "/" + variable + ".json", payLoad).Then(newValue =>
                {
                    Debug.Log("Updated value of " + variable + " in Chapter" + chapterNumber + " to " + payLoad.Trim('"'));
                }).Catch(error =>
                {
                    Debug.Log("Error: " + error);
                });
            }).Catch(error =>
            {
                Debug.Log("Error: " + error);
            });
        }
        
        public static void GetChapter1FromDatabase(GetChapterCallback callback, GetChapterFallback fallback)
        {
            RestClient.Get<Chapter1>(DatabaseUrl + "Chapter1.json").Then(chapter =>
                {
                    callback(chapter);
                }).Catch(error =>
            {
                Debug.Log("Error: " + error);
                fallback();
            });
        }
    }
}
