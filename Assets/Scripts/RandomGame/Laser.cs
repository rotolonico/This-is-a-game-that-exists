using System;
using Handlers;
using UnityEngine;

namespace RandomGame
{
    public class Laser : MonoBehaviour
    {
        public Laser parent;
        public GameObject square;
        public Vector2 squarePosition = new Vector2(10000,10000);
        
        public GameObject laser;
        public Vector3 direction;

        private Laser oldLaser;
        private Transform squareTransform;
        private LineRenderer lineRenderer;
        private Transform t;

        private bool reflected;
        private bool destroy;

        private void Start()
        {
            reflected = false;
            t = transform;
            lineRenderer = GetComponent<LineRenderer>();
            var position = t.position;
            lineRenderer.SetPosition(0, position);
            lineRenderer.SetPosition(1, position);
            squareTransform = square.transform;
        }

        private void Update()
        {
            if (squarePosition != new Vector2(10000, 10000) && squarePosition != (Vector2)squareTransform.position)
            {
                parent.reflected = false;
                destroy = true;
            }
            
            var hit = Physics2D.Raycast(t.position, direction, Mathf.Infinity, 1 << 8);
            var normal = new Vector2(hit.normal.x, hit.normal.y);            
            var angle = Vector3.Angle(normal, direction) * Mathf.Sign(normal.x * direction.y - normal.y * direction.x);
            
            if (!reflected && oldLaser != null && oldLaser.destroy) Destroy(oldLaser.gameObject);
            if (hit.collider && hit.collider.CompareTag("Square") && hit.collider.gameObject != square)
            {
                if (!reflected)
                {
                    SoundHandler.sound.Play(SoundHandler.sound.laser);
                    var newLaser = Instantiate(laser, hit.point, Quaternion.identity).GetComponent<Laser>();
                    var newDirection = transform.TransformDirection((Vector2)(Quaternion.Euler(0,0,angle) * Vector2.right));
                    if (angle * newDirection.x >= 0)
                    {
                        newDirection.x = -newDirection.x;
                        newDirection.y = -newDirection.y;
                    }

                    var laserTransform = newLaser.transform;
                    var laserPosition = laserTransform.position;
                    laserPosition = new Vector3(laserPosition.x, laserPosition.y, -1);
                    laserTransform.position = laserPosition;
                    newLaser.parent = gameObject.GetComponent<Laser>();
                    newLaser.direction = newDirection;
                    newLaser.square = hit.collider.gameObject;
                    newLaser.squarePosition = hit.transform.position;
                    
                    oldLaser = newLaser;
                    reflected = true;
                }
                lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));
            } else if (hit.collider && hit.collider.CompareTag("SquareTarget"))
            {
                StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().FinishSecondPuzzle());
                Destroy(GameObject.FindGameObjectWithTag("Square"));
                GameObject.FindGameObjectWithTag("SquareTarget").GetComponent<Square>().ChangeColor("Green");
            }
            else
            {
                lineRenderer.SetPosition(1, direction * 2000);
            }
        }
    }
}
