using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Effects
{
    public class EffectsController : MonoBehaviour
    {
        [BoxGroup("Effects List")]
        public List<Effect> Effects;

        [Button("Play")]
        public void Play()
        {
            foreach ( Effect effect in Effects )
            {
                effect.Play();
            }
        }
    }
}
