using System;
using System.Collections;
using UnityEngine;

namespace SimonGame
{
    public class SPlayerController : MonoBehaviour
    {
        public bool isActive;

        public AudioSource jumpSound;
        
        public float jumpTime;
        public float jumpHeight;
        public float jumpDelay;

        public Sprite sleepSprite;
        public Sprite moveSprite;
        
        public bool onGround;
        public bool onCelling;
        public bool reverseGravity;

        private Vector3 jumpVector;
        private bool isJumping;
        
        private Transform cameraTransform;
        
        private SpriteRenderer sr;
        private bool sleeping;
        
        
        private Rigidbody2D rb;
        void Start()
        {
            cameraTransform = Camera.main.transform;
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            jumpVector = new Vector3(0, jumpHeight, 0);
        }

        void Update()
        {
            var position = transform.position;
            cameraTransform.position = new Vector3(position.x, position.y, -1);
            
            if (!isActive) return;
            if (!sleeping && Math.Abs(Input.GetAxisRaw("Horizontal")) < 0.1 && Math.Abs(Input.GetAxisRaw("Vertical")) < 0.1)
            {
                sr.sprite = sleepSprite;
                sleeping = true;
            }
            else if (sleeping && (Math.Abs(Input.GetAxisRaw("Horizontal")) > 0.1 || Math.Abs(Input.GetAxisRaw("Vertical")) > 0.1))
            {
                sr.sprite = moveSprite;
                sleeping = false;
            }
            
            rb.MovePosition(position + new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0));
            if ((reverseGravity ? onCelling : onGround) && Input.GetAxisRaw("Vertical") > 0.5) StartCoroutine(JumpTimer());
            if (!isJumping || !onGround && !onCelling) return;
            isJumping = false;
            StopAllCoroutines();
        }

        public void InvertGravity()
        {
            rb.gravityScale = -100;
            reverseGravity = true;
        }

        private IEnumerator JumpTimer()
        {
            jumpSound.Play();
            isJumping = true;
            StartCoroutine(Jump());
            yield return new WaitForSeconds(jumpTime);
            isJumping = false;
        }

        private IEnumerator Jump()
        {
            while (isJumping)
            {
                if (!reverseGravity)
                {
                    rb.MovePosition(transform.position + jumpVector);
                    yield return new WaitForSeconds(jumpDelay);
                }
                else
                {
                    rb.MovePosition(transform.position - jumpVector);
                    yield return new WaitForSeconds(jumpDelay);
                }

                StartCoroutine(Jump());
            }
        }
    }
}
