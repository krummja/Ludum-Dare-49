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
        public static CameraManager Instance;

        public Camera StoryCamera;
        public Camera ShopCamera;

        public CameraType CurrentCamera { get; private set; }

        public void SwitchCamera()
        {
            StoryCamera.gameObject.SetActive(!StoryCamera.gameObject.activeSelf);
            ShopCamera.gameObject.SetActive(!ShopCamera.gameObject.activeSelf);
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

        private void Start()
        {
            CurrentCamera = CameraType.Story;
        }
    }
}
