using UnityEngine;
using UnityEngine.UI;

namespace LD49.GUIElements
{
    using Managers;

    public class Navigation : MonoBehaviour
    {
        public GameObject Options;

        public void ShowMenu()
        {
            Time.timeScale = 0;
            Options.SetActive(true);
        }

        public void HideMenu()
        {
            Options.SetActive(false);
        }
    }
}
