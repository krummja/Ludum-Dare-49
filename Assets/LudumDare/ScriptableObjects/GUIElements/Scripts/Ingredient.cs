using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    [CreateAssetMenu(fileName = "New Ingredient", menuName = "GUIElements/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public string Name;
        public bool IsUsed = false;

        [PreviewField(75)]
        public Sprite Icon;

        public int UsedAt { get; set; }
    }
}
