using System;
using UnityEngine;

namespace GirlGame
{
    public class EditorHandler : MonoBehaviour
    {
        public static bool activated;
        
        public GameObject Wall;

        private Camera mainCamera;
        private bool leaveEditorButton;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (!activated) return;
            if (Input.GetMouseButton(0))
            {
                var v3 = Input.mousePosition;
                v3.z = 10.0f;
                var hit = new Collider2D[1];
                Physics2D.OverlapCircleNonAlloc(mainCamera.ScreenToWorldPoint(v3), 0.25f, hit);
                if (hit[0] == null) Instantiate(Wall, mainCamera.ScreenToWorldPoint(v3), Quaternion.identity);
            }
            
            if (Input.GetMouseButton(1))
            {
                var v3 = Input.mousePosition;
                v3.z = 10.0f;
                var hits = new Collider2D[5];
                Physics2D.OverlapCircleNonAlloc(mainCamera.ScreenToWorldPoint(v3), 0.25f, hits);
                foreach (var hit in hits)
                {
                    try
                    {
                        if (hit.CompareTag("GPlayer")) StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().DeletePlayer());
                        if (hit.CompareTag("GPlayer") || hit.CompareTag("GWall")) Destroy(hit.gameObject);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
        
        public void LeaveEditor()
        {
            if (!leaveEditorButton) StartCoroutine(GameObject.Find("MainHandler").GetComponent<Main>().LeaveEditor());
            leaveEditorButton = true;
        }
    }
}
