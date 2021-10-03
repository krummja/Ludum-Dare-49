using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    public enum Expression
    {
        Neutral,
        Happy,
        Angry,
        Sad,
        Failure,
    }

    public class StoryAnimator : MonoBehaviour
    {
        public List<Sprite> Sprites;
        public bool IsMochi = true;

        [EnumToggleButtons, OnValueChanged("SwitchSprite"), DisableInEditorMode]
        public Expression Expression;

        private Image renderer;

        private static readonly int Neutral = 0;
        private static readonly int Happy = 1;
        private static readonly int Angry = 2;
        private static readonly int Sad = 3;
        private static readonly int Failure = 4;

        private void Start()
        {
            renderer = GetComponent<Image>();
        }

        public void SwitchSprite()
        {
            switch ( Expression )
            {
                case Expression.Neutral:
                {
                    renderer.sprite = Sprites[Neutral];
                    break;
                }

                case Expression.Happy:
                {
                    renderer.sprite = Sprites[Happy];
                    break;
                }

                case Expression.Angry:
                {
                    renderer.sprite = Sprites[Angry];
                    break;
                }

                case Expression.Sad:
                {
                    renderer.sprite = Sprites[Sad];
                    break;
                }

                case Expression.Failure:
                {
                    if ( IsMochi )
                    {
                        renderer.sprite = Sprites[Failure];
                    }

                    else
                    {
                        renderer.sprite = Sprites[Angry];
                    }
                    break;
                }
            }
        }
    }
}
