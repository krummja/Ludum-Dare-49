using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Screens
{
    using GUIElements;

    public class StoryScreen : MonoBehaviour
    {
        public TeleType TeleType;
        public StoryText Script;

        [ResponsiveButtonGroup("TextControl", UniformLayout = true)]
        public void Play()
        {
            if ( TeleType.GetComponent<TeleType>().StoryText == null )
            {
                TeleType.GetComponent<TeleType>().StoryText = Script;
            }
            TeleType.gameObject.SetActive(true);
            TeleType.GetComponent<TeleType>().Play();
        }

        [ResponsiveButtonGroup("TextControl")]
        public void Stop()
        {
            TeleType.GetComponent<TeleType>().Stop();
            TeleType.gameObject.SetActive(false);
        }

        [ResponsiveButtonGroup("TextControl")]
        public void Next()
        {
            TeleType.GetComponent<TeleType>().Next();
        }

        private void Start()
        {
            Script.CurrentIndex = 0;
        }
    }
}
