using UnityEngine;

namespace LD49.Screens
{
    using Managers;

    public class MainMenu : MonoBehaviour
    {
        public GameObject OptionsMenu;

        public void OnStartPressed()
        {
            GameManager.Instance.TransitionToState(GameState.Intro);
        }

        public void OnOptionsPressed()
        {
            OptionsMenu.SetActive(true);
        }

        public void OnQuitPressed()
        {
            GameManager.Instance.QuitApplication();
        }

        public void Options_OnBackPressed()
        {
            OptionsMenu.SetActive(false);
        }
    }
}
