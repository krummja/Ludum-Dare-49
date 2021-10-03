using UnityEngine;
using ElRaccoone.Tweens;

namespace LD49.Effects
{
    using System.Collections;

    public class FloatingBook : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(HoverUp());
        }

        private void SwitchToUp()
        {
            Debug.Log("Up");
            StartCoroutine(HoverUp());
        }

        private void SwitchToDown()
        {
            Debug.Log("Down");
            StartCoroutine(HoverDown());
        }

        private IEnumerator HoverUp()
        {
            yield return this.gameObject.TweenPositionY(2.8f, 5).SetOnComplete(SwitchToDown);
        }

        private IEnumerator HoverDown()
        {
            yield return this.gameObject.TweenPositionY(2.6f, 5).SetOnComplete(SwitchToUp);
        }
    }
}
