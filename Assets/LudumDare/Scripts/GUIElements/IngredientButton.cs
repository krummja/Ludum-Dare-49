using UnityEngine;
using UnityEngine.UI;

namespace LD49.GUIElements
{
    using Screens;

    public class IngredientButton : MonoBehaviour
    {
        public Ingredient Ingredient;
        public CraftingTable Table;

        public bool IsSelected = false;

        private Image _image;
        private Button _button;

        public int Index { get; set; }

        public void OnClicked()
        {
            if ( !Ingredient.IsUsed )
            {
                Table.AddIngredientToSlot(Ingredient);
            }
            else
            {
                Table.ClearSlot(Ingredient.UsedAt);
            }
        }

        private void Start()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);

            Ingredient.IsUsed = false;
        }

        private void Update()
        {
            if ( Ingredient.IsUsed )
            {

            }
            else
            {

            }
        }
    }
}
