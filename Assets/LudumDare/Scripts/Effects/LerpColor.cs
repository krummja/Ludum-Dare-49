using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Effects
{
    using System;

    public class LerpColor : Effect
    {
        public String TargetProperty = "_Color";
        public Color TargetColor;
        public float Duration;

        private Material _material;

        private float _time;
        private Color _startValue;

        [Button("Play")]
        public override void Play()
        {
            _material.SetColor(TargetProperty, _startValue);
            _time = 0;
        }

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void Start()
        {
            _time = 0;
            _startValue = _material.GetColor(TargetProperty);
        }

        private void Update()
        {
            if ( _time < Duration ) {
                Color changedColor = Color.Lerp(_startValue, TargetColor, _time / Duration);
                _material.SetColor(TargetProperty, changedColor);
                _time += Time.deltaTime;
            }

            else {
                _material.SetColor(TargetProperty, TargetColor);
            }
        }
    }
}
