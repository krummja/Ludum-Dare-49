using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Screens
{
    using GUIElements;
    using Managers;

    public class StoryScreen : AbstractScreen
    {
        [BoxGroup("Component Dependencies")]
        public TeleType TeleType;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator MochiAnimator;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator BunnerlyAnimator;
        [BoxGroup("Component Dependencies")]
        public GameObject ForwardButton;
        [BoxGroup("Component Dependencies")]
        public GameObject GameOver;

        [BoxGroup("Script")]
        public GameObject MochiPanel;
        [BoxGroup("Script")]
        public GameObject BunnerlyPanel;
        [BoxGroup("Script")]
        public StoryText IntroScript;
        [BoxGroup("Script")]
        public StoryText StageOneScript;
        [BoxGroup("Script")]
        public StoryText StageTwoScript;
        [BoxGroup("Script")]
        public StoryText StageThreeScript;
        [BoxGroup("Script")]
        public StoryText FailScript;
        [BoxGroup("Script")]
        public StoryText WinScript;

        private Speaker _current;

        public StoryText Script { get; private set; }

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
            if ( Script == null )
            {
                Script = StageOneScript;
            }

            TeleType.gameObject.SetActive(true);

            switch ( GameManager.Instance.StoryStage )
            {
                case StoryStage.Intro:
                    BunnerlyAnimator.gameObject.SetActive(false);
                    Script = IntroScript;
                    break;
                case StoryStage.FirstPotion:
                    BunnerlyAnimator.gameObject.SetActive(true);
                    Script = StageOneScript;
                    break;
                case StoryStage.SecondPotion:
                    Script = StageTwoScript;
                    break;
                case StoryStage.ThirdPotion:
                    Script = StageThreeScript;
                    break;
                case StoryStage.Win:
                    Script = WinScript;
                    ForwardButton.SetActive(false);
                    break;
                case StoryStage.Lose:
                    Script = FailScript;
                    ForwardButton.SetActive(false);
                    break;
            }

            Script.CurrentIndex = 0;
            Script.Complete = false;

            TeleType.GetComponent<TeleType>().StoryText = Script;

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
                if ( Script == IntroScript )
                {
                    if (Script.Complete)
                    {
                        GameManager.Instance.StoryStage = StoryStage.FirstPotion;
                        GameManager.Instance.TransitionToState(GameState.Transition);
                    }
                }

                else
                {
                    GameManager.Instance.TransitionToState(GameState.Shop);
                }
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

        private void Update()
        {
            if ( Script == FailScript )
            {
                if ( Script.Complete ) GameOver.SetActive(true);
                else GameOver.SetActive(false);
            }
        }

    }
}
