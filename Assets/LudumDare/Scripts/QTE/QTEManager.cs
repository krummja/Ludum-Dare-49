using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LD49.QTE
{
    using System;
    using Effects;
    using ElRaccoone.Tweens;
    using GUIElements;
    using Managers;
    using TMPro;

    public class QTEManager : MonoBehaviour
    {
        private static readonly int Brewing = Animator.StringToHash("brewing");
        private static readonly int Thinking = Animator.StringToHash("thinking");
        private static readonly int Shake = Animator.StringToHash("succeed");
        private static readonly int Fail = Animator.StringToHash("fail");

        public GameObject Indicator;
        public GameObject Icon;
        public TextMeshProUGUI Nice;
        public TextMeshProUGUI Bad;
        public TextMeshProUGUI Missed;
        public float SlowMotionTimeScale = 0.1f;
        public Animator Animator;

        public CraftingTable Table;

        public List<int> Sequence;

        private bool isStarted;
        private QTEEvent eventData;

        private bool isAllIngredientsUsed;
        private bool isFail;
        private bool isEnded;
        private bool isPaused;
        private bool wrongButtonPressed;
        private int sequenceIndex = 0;

        private float currentTime;
        private float smoothTimeUpdate;
        private float rememberTimeScale;

        private bool window;
        private bool missed;

        private List<Color> Colors = new List<Color>()
        {
            Color.red,
            Color.green,
            Color.magenta,
            Color.blue,
            Color.cyan,
            Color.yellow
        };

        public int SequenceIndex
        {
            get => sequenceIndex;
            set => sequenceIndex = value;
        }

        public void StartEvent(QTEEvent eventScriptable)
        {
            Animator.SetBool(Brewing, true);

            Nice.gameObject.SetActive(false);
            Bad.gameObject.SetActive(false);
            Missed.gameObject.SetActive(false);

            eventData = eventScriptable;

            for ( int i = 0; i < 10; i++ )
            {
                Sequence.Add(eventData.Challenge.ChallengeArray[i]);
            }

            if ( eventData.onStart != null )
            {
                eventData.onStart.Invoke();
            }

            isAllIngredientsUsed = false;
            isEnded = false;
            isFail = false;
            isPaused = false;
            rememberTimeScale = Time.timeScale;

            switch ( eventScriptable.TimeType )
            {
                case QTETimeType.Slow:
                {
                    Time.timeScale = SlowMotionTimeScale;
                    break;
                }

                case QTETimeType.Paused:
                {
                    Time.timeScale = 0f;
                    break;
                }
            }

            currentTime = eventData.Time;
            smoothTimeUpdate = currentTime;

            window = false;
            isStarted = true;

            sequenceIndex = 0;
            StartCoroutine(Stage(sequenceIndex));
        }

        public void CheckInput(int buttonIndex)
        {
            bool succeed = eventData.Challenge.ChallengeArray[sequenceIndex] == buttonIndex;

            if ( window && succeed )
            {
                MeshRenderer r = Table.PotionSurface.GetComponent<MeshRenderer>();
                Random random = new Random();
                Color c = Colors[random.Next(0, 5)];
                r.material.color = c;

                ParticleSystem.MainModule s1 = Table.BrewBubble.main;
                s1.startColor = new ParticleSystem.MinMaxGradient(c);

                Table.SoundEffects.clip = Table.BubbleSound;
                Table.SoundEffects.Play();

                Table.OverBubble.Play();
                ParticleSystem.MainModule s2 = Table.OverBubble.main;
                s2.startColor = new ParticleSystem.MinMaxGradient(c);

                Animator.SetTrigger(Shake);
                Nice.gameObject.SetActive(true);
                missed = false;
                isFail = false;
            }
            else if ( window && !succeed )
            {
                Bad.gameObject.SetActive(true);
                Table.ChancesInRound -= 1;
            }
            else
            {
                Missed.gameObject.SetActive(true);
                missed = true;
                Table.ChancesInRound -= 1;
            }
        }

        protected void Update()
        {
            if ( Table.ShopState != ShopState.Selecting )
            {
                Animator.SetBool(Thinking, false);
            }

            if ( !isStarted || eventData == null || isPaused ) return;

            if ( Table.ChancesInRound <= 0 )
            {
                isFail = true;
            }

            if ( isFail )
            {
                Animator.SetBool(Brewing, false);
                StopAllCoroutines();
                DoFinally();
            }

            if ( sequenceIndex == 10 )
            {
                isAllIngredientsUsed = true;
                DoFinally();
            }
        }

        protected void DoFinally()
        {
            isEnded = true;
            isStarted = false;
            Time.timeScale = rememberTimeScale;

            if ( eventData.onEnd != null )
                eventData.onEnd.Invoke();
            if (eventData.onFail != null && isFail)
                eventData.onFail.Invoke();
            if (eventData.onSuccess != null && isAllIngredientsUsed)
                eventData.onSuccess.Invoke();

            Table.ShopState = ShopState.Result;
            eventData = null;
        }

        private IEnumerator Stage(int index)
        {
            int _index = index;
            float _time = eventData.Timings[index];
            currentTime = 0f;

            Nice.gameObject.SetActive(false);
            Bad.gameObject.SetActive(false);
            Missed.gameObject.SetActive(false);

            while ( currentTime < _time - 1 )
            {
                Indicator.GetComponent<EffectsController>().Play();
                currentTime++;
                yield return new WaitForSecondsRealtime(1f);
            }

            float _exitTime = 0f;

            int iconIndex = eventData.Challenge.ChallengeArray[sequenceIndex];
            Icon.GetComponent<Image>().sprite = Table.Buttons[iconIndex].TableSlot.FilledIcon;
            Icon.GetComponent<Image>().preserveAspect = true;
            Icon.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            missed = false;

            float _timing;

            if ( GameManager.Instance.StoryStage == StoryStage.FirstPotion ) _timing = 1.5f;
            else if ( GameManager.Instance.StoryStage == StoryStage.SecondPotion ) _timing = 1f;
            else if ( GameManager.Instance.StoryStage == StoryStage.ThirdPotion ) _timing = 0.75f;
            else _timing = 3f;  // Debug timing

            while ( _exitTime < _timing )
            {
                window = true;
                _exitTime += Time.deltaTime;
                yield return null;
            }

            Icon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Icon.GetComponent<Image>().sprite = null;

            window = false;

            if ( missed )
            {
                CheckInput(-1);
            }

            sequenceIndex++;
            if ( sequenceIndex < 10 && !isFail )
            {
                StartCoroutine(Stage(sequenceIndex));
            }
        }
    }
}
