using UnityEngine;

namespace GirlGame
{
    public class GPlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;

        public static bool activated;

        public int speed = 1000;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (activated) rb.MovePosition(transform.position + new Vector3 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*Time.deltaTime*speed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("GEnd")) return;
            StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().EndGirlGame());
            activated = false;
            GetComponent<SpriteRenderer>().color = Color.clear;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
