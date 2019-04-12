using System.Linq;
using GirlGame;
using Handlers;
using UnityEngine;

namespace RandomGame
{
    public class SquareHolder : MonoBehaviour
    {
        public bool filled;
        private string squareColor;
        public string color;

        private Rigidbody2D squareRb;
        private GameObject[] squareHolders;

        private void Start()
        {
            squareHolders = GameObject.FindGameObjectsWithTag("SquareHolder");
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (filled || !other.gameObject.CompareTag("Square") || other.gameObject.GetComponent<Rigidbody2D>() == ClickHandler.DraggedColliderRb) return;
            squareRb = other.gameObject.GetComponent<Rigidbody2D>();
            filled = true;
            var otherTransform = other.transform;
            otherTransform.position = (Vector2) transform.position;
            otherTransform.rotation = transform.rotation;
            other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            squareColor = other.GetComponent<Square>().color;
            CheckHolders();
        }

        private void Update()
        {
            if (filled && Input.GetMouseButton(0) && ClickHandler.DraggedColliderRb == squareRb)
            {
                filled = false;
                squareRb = null;
            }
        }

        private void CheckHolders()
        {
            foreach (var squareHolder in squareHolders)
            {
                var squareHolderComponent = squareHolder.GetComponent<SquareHolder>();
                if (squareHolderComponent.filled == false) return;
                if (squareHolderComponent.squareColor != squareHolderComponent.color) return;
            }

            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().FinishFirstPuzzle());
        }
    }
}
