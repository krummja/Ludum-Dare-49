using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        public GameObject OptionsMenu;

        public abstract void OnOptionsPressed();

        public abstract void Options_OnReturnPressed();

        public abstract void Options_OnQuitPressed();
    }
}
