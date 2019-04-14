using UnityEngine;

namespace RandomGame
{
    public class Laser : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private Transform t;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            t = transform;
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, t.right - t.up);
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));
            }
            else
            {
                lineRenderer.SetPosition(1, t.up * 2000);
            }
        }
    }
}
