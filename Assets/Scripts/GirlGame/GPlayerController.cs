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
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (activated) rb.MovePosition(transform.position + new Vector3 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*Time.deltaTime*speed);
        }
    }
}
