using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    using System;
    using Screens;
    using UnityEngine.UIElements;

    public class OptionsMenu : MonoBehaviour
    {
        public AbstractScreen Context;
        public ButtonList Buttons;

        public int Y_OFFSET = 75;

        private void Start()
        {
            SetupButtonList();
        }

        private void SetupButtonList()
        {
            for ( int i = 0; i < Buttons.Buttons.Count; i++ )
            {
                GameObject button = Buttons.Buttons[i];
                GameObject _button = Instantiate(button, Vector2.zero, Quaternion.identity);
                _button.transform.parent = this.transform;
                _button.transform.localPosition = new Vector2(0f, -(i * Y_OFFSET) + 100f);
            }
        }
    }
}

