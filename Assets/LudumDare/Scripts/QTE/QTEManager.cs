using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LD49.QTE
{
    using System;
    using GUIElements;

    public class QTEManager : MonoBehaviour
    {
        public GameObject Indicator;
        public float SlowMotionTimeScale = 0.1f;

        public CraftingTable Table;

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

        private List<Color> Colors = new List<Color>()
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.cyan,
            Color.magenta
        };

        public void StartEvent(QTEEvent eventScriptable)
        {
            eventData = eventScriptable;

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
            StartCoroutine(Stage(sequenceIndex));
            // StartCoroutine(CountDown());
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Play()
        {
            isPaused = false;
        }

        public void CheckInput(int buttonIndex)
        {
            Debug.Log(sequenceIndex);
            bool succeed = eventData.Challenge.ChallengeArray[sequenceIndex] == buttonIndex;
            if ( window && succeed )
            {
                isFail = false;
            }
            else
            {
                isFail = true;
            }
        }

        protected void Update()
        {
            if ( !isStarted || eventData == null || isPaused ) return;

            UpdateTimer();

            if ( isFail )
            {
                DoFinally();
            }

            if ( sequenceIndex == 9 )
            {
                isAllIngredientsUsed = true;
                DoFinally();
            }
        }

        protected void DoFinally()
        {
            // Win Condition

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

        protected void UpdateTimer()
        {

        }

        private IEnumerator Stage(int index)
        {
            int _index = index;
            float _time = eventData.Timings[index];
            currentTime = 0f;

            while ( currentTime < _time - 1 )
            {
                StartCoroutine(Blink());
                currentTime++;
                yield return new WaitForSecondsRealtime(1f);
            }

            Debug.Log("Finished Blink");

            float _exitTime = 0f;

            Random random = new Random();
            Indicator.GetComponent<Image>().color = Colors[random.Next(0, 4)];

            while ( _exitTime < 0.4f )
            {

                window = true;
                _exitTime += Time.deltaTime;
                yield return null;
            }

            window = false;
            CheckInput(-1);

            Debug.Log("Next Sequence");

            sequenceIndex += 1;
            if (sequenceIndex < 10)
            {
                StartCoroutine(Stage(sequenceIndex));
            }
        }

        private IEnumerator Blink()
        {
            float localTime = 0;

            while ( localTime < 1 )
            {
                Indicator.GetComponent<Image>().color = Color.Lerp(Color.black, Color.white, localTime);
                localTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator CountDown()
        {
            isStarted = true;
            Random random = new Random();

            while ( currentTime > 0 && isStarted && !isEnded )
            {

                // Indicator.GetComponent<Image>().color = Colors[random.Next(0, 4)];

                currentTime--;
                yield return new WaitWhile(() => isPaused);
                yield return new WaitForSecondsRealtime(1f);
            }

            if ( !isAllIngredientsUsed && isEnded )
            {
                isFail = true;
                DoFinally();
            }
        }
    }
}
