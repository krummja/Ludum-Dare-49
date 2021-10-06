using UnityEngine;

namespace LD49.Screens
{
    using Managers;

    public class MainMenu : AbstractScreen
    {
        public void OnStartPressed()
        {
            GameManager.Instance.TransitionToState(GameState.Intro);
        }
    }
}
