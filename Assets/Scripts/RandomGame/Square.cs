using UnityEngine;

namespace RandomGame
{
    public class Square : MonoBehaviour
    {
        public string color;

        private SpriteRenderer sr;

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void ChangeColor(string newColor)
        {
            color = newColor;
            
            switch (newColor)
            {
                case "White":
                    sr.color = Color.white;
                    break;
                case "Green":
                    sr.color = Color.green;
                    break;
                case "Red":
                    sr.color = Color.red;
                    break;
            }
        }
    }
}
