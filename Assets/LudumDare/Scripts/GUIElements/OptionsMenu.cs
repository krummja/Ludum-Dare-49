using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    using System;
    using Managers;
    using Screens;

    public class OptionsMenu : MonoBehaviour
    {
        public void OnReturnPressed()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void OnQuitPressed()
        {
            switch ( GameManager.Instance.CurrentState )
            {
                case GameState.Menu:
                {
                    GameManager.Instance.QuitApplication();
                    break;
                }

                default:
                {
                    GameManager.Instance.TransitionToState(GameState.Menu);
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}

