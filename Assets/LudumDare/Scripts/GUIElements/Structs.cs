using UnityEngine;
using UnityEngine.UI;

namespace LD49.GUIElements
{
    // public struct Ingredient
    // {
    //     public string Name;
    //     public Image Icon;
    // }

    // public struct TableSlot
    // {
    //     public Ingredient Ingredient;
    //     public bool IsEmpty;
    // }

    public struct IngredientBinding
    {
        public Ingredient Ingredient;
        public TableSlot TableSlot;
    }
}
