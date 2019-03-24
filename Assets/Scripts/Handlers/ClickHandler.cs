using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Handlers
{
    public class ClickHandler : MonoBehaviour
    {
        public static bool Active;
        
        public static Vector3 MousePositionI;
        public static Rigidbody2D DraggedColliderRb;
        public static Vector3 DraggedGameObjectPositionI;
        public static bool isDragging;

        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            if (!Active) return;
            if (Input.GetMouseButtonDown(0))
            {
                var v3 = Input.mousePosition;
                MousePositionI = camera.ScreenToWorldPoint(v3);
                MousePositionI.z = 10;

                var hitCollider = Physics2D.OverlapCircleAll(MousePositionI, 0.01f);
                foreach (var col in hitCollider)
                {
                    if (col.GetComponent<Draggable>() == null) continue;
                    DraggedColliderRb = col.GetComponent<Rigidbody2D>();
                    DraggedGameObjectPositionI = DraggedColliderRb.transform.position;
                    isDragging = true;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (isDragging)
                {
                    var mousePosition = Input.mousePosition;
                    mousePosition.z = 10.0f;
                    mousePosition = camera.ScreenToWorldPoint(mousePosition);

                    var newPosition = mousePosition + (DraggedGameObjectPositionI - MousePositionI);
                    newPosition.z = 10;
                    DraggedColliderRb.velocity = (newPosition - DraggedColliderRb.transform.position) * 10;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                DraggedColliderRb = null;
                DraggedGameObjectPositionI = Vector3.zero;
            }
        }
    }
}
