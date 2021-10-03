using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    [CreateAssetMenu(fileName = "New Table Slot", menuName = "GUIElements/Table Slot")]
    public class TableSlot : ScriptableObject
    {
        public Ingredient Ingredient;

        [PreviewField(75)]
        public Sprite Icon;

        [PreviewField(75)]
        public Sprite FilledIcon;
    }
}
