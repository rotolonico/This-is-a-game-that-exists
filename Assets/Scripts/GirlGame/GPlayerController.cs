using UnityEngine;

namespace GirlGame
{
    public class GPlayerController : MonoBehaviour
    {
        public static bool activated;
        public static bool editorMode;

        public int speed = 1000;
        
        private Rigidbody2D rb;
        private Main main;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            main = GameObject.Find("MainHandler").GetComponent<Main>();
        }

        private void Update()
        {
            if (activated) rb.MovePosition(transform.position + new Vector3 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*Time.deltaTime*speed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("GEnd")) return;
            StartCoroutine(!editorMode ? main.EndGirlGame() : main.FinishEditor());
            activated = false;
            GetComponent<SpriteRenderer>().color = Color.clear;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
