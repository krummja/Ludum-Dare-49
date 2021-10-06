using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Effects
{
    public class LerpScale : Effect
    {
        public float TargetScale;
        public float Duration;

        private float _scaleModifier = 1f;

        private float _time;
        private float _startValue;
        private Vector3 _startScale;


        public override void Play()
        {
            _scaleModifier = _startValue;
            transform.localScale = _startScale;
            _time = 0;
        }

        private void Start()
        {
            _time = 0;
            _startValue = _scaleModifier;
            _startScale = transform.localScale;
        }

        private void Update()
        {
            if ( _time < Duration ) {
                _scaleModifier = Mathf.Lerp(_startValue, TargetScale, _time / Duration);
                transform.localScale = _startScale * _scaleModifier;

                _time += Time.deltaTime;
            }

            else {
                transform.localScale = _startScale * TargetScale;
                _scaleModifier = TargetScale;
            }
        }
    }
}
