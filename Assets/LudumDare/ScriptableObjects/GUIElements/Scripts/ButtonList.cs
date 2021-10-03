using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    using Managers;
    using UnityEngine.UI;

    [CreateAssetMenu(fileName = "New Button List", menuName = "GUIElements/Button List")]
    public class ButtonList : ScriptableObject
    {
        public List<GameObject> Buttons;
    }
}
