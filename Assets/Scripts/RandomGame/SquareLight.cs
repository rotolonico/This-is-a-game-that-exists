using Handlers;
using UnityEngine;

namespace RandomGame
{
    public class SquareLight : MonoBehaviour
    {
        public string color;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var square = other.GetComponent<Square>();
            if (square.color == color) return;
            SoundHandler.sound.PlaySecondary(SoundHandler.sound.squareColor);
            square.ChangeColor(color);
        }
    }
}
