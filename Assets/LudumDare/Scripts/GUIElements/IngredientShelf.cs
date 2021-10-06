using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LD49.GUIElements
{
    public class IngredientShelf : SerializedMonoBehaviour
    {
        public CraftingTable Table;

        public List<IngredientButton> Buttons;

        public int SelectedCount = 0;

        private void Start()
        {
            Buttons = new List<IngredientButton>();
            for ( int i = 0; i < transform.childCount; i++ )
            {
                GameObject child = transform.GetChild(i).gameObject;
                IngredientButton button = child.GetComponent<IngredientButton>();
                Debug.Log(button.Ingredient.Name);
                Buttons.Add(button);
            }
        }
    }


}
