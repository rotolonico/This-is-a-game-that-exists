using UnityEngine;

namespace SimonGame
{
    public class SCollider : MonoBehaviour
    {
        public AudioSource loseSound;
        
        public bool isGroundCol;
        
        private SPlayerController sPlayerController;
        private Transform startTransform;

        private void Start()
        {
            startTransform = GameObject.Find("StartPosition").transform;
            sPlayerController = gameObject.GetComponentInParent<SPlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name != "EndPosition") return;
            sPlayerController.isActive = false;
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().FinishSimonGame());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("SRed")) return;
            loseSound.Play();
            transform.parent.position = startTransform.position;
        }

        private void OnCollisionStay2D()
        {
            if (isGroundCol)
            {
                sPlayerController.onGround = true;
            }
            else
            {
                sPlayerController.onCelling = true;
            }
        }
        
        private void OnCollisionExit2D()
        {
            if (isGroundCol)
            {
                sPlayerController.onGround = false;
            }
            else
            {
                sPlayerController.onCelling = false;
            }
        }
    }
}
