using UnityEngine;

namespace LD49.Managers
{
    public enum CameraType
    {
        Story,
        Shop,
    }

    public class CameraManager : MonoBehaviour
    {
        public Camera StoryCamera;
        public Camera ShopCamera;

        public static CameraManager Instance;

        public CameraType CurrentCamera { get; private set; }

        public void SwitchCamera()
        {
            switch ( CurrentCamera )
            {
                case CameraType.Story:
                {
                    StoryCamera.enabled = true;
                    ShopCamera.enabled = false;
                    CurrentCamera = CameraType.Shop;
                    break;
                }

                case CameraType.Shop:
                {
                    StoryCamera.enabled = false;
                    ShopCamera.enabled = true;
                    CurrentCamera = CameraType.Story;
                    break;
                }
            }
        }

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
    }
}
