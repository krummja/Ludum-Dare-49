using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Screens
{
    using GUIElements;
    using Managers;

    public class StoryScreen : MonoBehaviour
    {
        [BoxGroup("Component Dependencies")]
        public TeleType TeleType;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator MochiAnimator;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator BunnerlyAnimator;

        [BoxGroup("Script")]
        public GameObject MochiPanel;
        [BoxGroup("Script")]
        public GameObject BunnerlyPanel;
        [BoxGroup("Script")]
        public StoryText Script;

        private Speaker _current;

        public Speaker CurrentSpeaker
        {
            get
            {
                return _current;
            }

            set
            {
                _current = value;
                TeleType.CurrentSpeaker = value;
            }
        }

        [ResponsiveButtonGroup("TextControl", UniformLayout = true)]
        public void Play()
        {
            if ( TeleType.GetComponent<TeleType>().StoryText == null )
            {
                TeleType.GetComponent<TeleType>().StoryText = Script;
            }
            TeleType.gameObject.SetActive(true);

            TeleType.MochiAnimator = MochiAnimator;
            TeleType.BunnerlyAnimator = BunnerlyAnimator;

            CurrentSpeaker = Script.SpeakerSequence[0];
            SwitchPanel();

            Script.StartExpressions(MochiAnimator, BunnerlyAnimator);
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
            if ( !Script.Complete )
            {
                TeleType teleType = TeleType.GetComponent<TeleType>();
                CurrentSpeaker = Script.SpeakerSequence[Script.CurrentIndex];
                SwitchPanel();

                Script.NextMochiExpression(MochiAnimator);
                Script.NextBunnerlyExpression(BunnerlyAnimator);
                teleType.Next();
            }
            else
            {
                GameManager.Instance.TransitionToState(GameState.Shop);
            }
        }

        private void SwitchPanel()
        {
            switch ( _current )
            {
                case Speaker.Mochi:
                {
                    MochiPanel.SetActive(true);
                    BunnerlyPanel.SetActive(false);
                    break;
                }

                case Speaker.Bunnerly:
                {
                    MochiPanel.SetActive(false);
                    BunnerlyPanel.SetActive(true);
                    break;
                }
            }
        }

        private void Start()
        {
            Script.CurrentIndex = 0;
            Play();
        }
    }
}
