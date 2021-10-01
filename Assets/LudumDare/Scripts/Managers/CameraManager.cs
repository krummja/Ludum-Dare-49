using UnityEngine;

namespace LD49.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public Camera Camera;

        public static CameraManager Instance;

        private void Awake()
        {
            if ( Instance != null )
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start() { }

        private void Update() { }
    }
}
