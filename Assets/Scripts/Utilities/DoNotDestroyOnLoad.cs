using UnityEngine;

namespace Utilities
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
