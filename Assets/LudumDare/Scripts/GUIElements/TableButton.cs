using UnityEngine;
using UnityEngine.UI;

namespace LD49.GUIElements
{
    using Screens;

    public class TableButton : MonoBehaviour
    {
        public TableSlot TableSlot;
        public CraftingTable Table;

        private IngredientShelf _shelf;
        private Image _image;
        private Button _button;

        public int Index { get; set; }

        public void OnClicked()
        {
            switch ( Table.ShopState )
            {
                case ShopState.Selecting:
                {
                    if (TableSlot.Ingredient)
                    {
                        Table.ClearSlot(Index);
                    }
                    break;
                }

                case ShopState.Brewing:
                {
                    Table.CaptureInput(Index);
                    break;
                }
            }

        }

        private void Start()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
            _image.sprite = TableSlot.Icon;

            TableSlot.Ingredient = null;
            TableSlot.FilledIcon = null;
        }

        private void Update()
        {
            if ( TableSlot.Ingredient )
            {
                _image.sprite = TableSlot.FilledIcon;
            }

            else
            {
                _image.sprite = TableSlot.Icon;
            }
        }
    }
}
