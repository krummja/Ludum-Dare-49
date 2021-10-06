using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource AudioSource;
        public AudioClip Music;

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
            AudioSource.clip = Music;
            AudioSource.Play();
        }

        private void Update() { }
    }
}
