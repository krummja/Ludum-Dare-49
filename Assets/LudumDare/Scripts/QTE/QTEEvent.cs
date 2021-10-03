using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;

using Random = System.Random;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LD49.QTE
{
    public enum QTETimeType
    {
        Normal,
        Slow,
        Paused
    }

    public enum QTEPressType
    {
        Single,
        Simultaneously
    }

    [System.Serializable]
    public class QTEInput
    {
        public int[] ChallengeArray;
        private int[] startArray = new int[] { 0, 1, 2, 3, 4, -1, -1, -1, -1, -1 };

        public void Randomize()
        {
            int[] arr = startArray;
            Random random = new Random();
            arr = arr.OrderBy(x => random.Next()).ToArray();

            for ( int i = 0; i < arr.Length; i++ )
            {
                arr[i] = arr[i] == -1 ? random.Next(0, 4) : arr[i];
            }

            ChallengeArray = arr;
        }
    }

    public class QTEEvent
    {
        public QTEInput Challenge = new QTEInput();
        public QTETimeType TimeType;
        public float Time;
        public int[] Timings;
        public float Window = 0.2f;

        public QTEPressType PressType;
        public bool FailOnWrongPress;

        public UnityEvent onStart;
        public UnityEvent onEnd;
        public UnityEvent onSuccess;
        public UnityEvent onFail;

        public void Start()
        {
            Challenge.Randomize();
            Timings = new int[10];

            Random random = new Random();
            for ( int i = 0; i < Timings.Length; i++ )
            {
                Timings[i] = random.Next(3, 5);
            }

            Time = Timings.Sum();
        }
    }
}
