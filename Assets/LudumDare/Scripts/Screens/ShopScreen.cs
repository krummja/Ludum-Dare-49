using UnityEngine;

namespace LD49.Screens
{
    using GUIElements;

    public class ShopScreen : AbstractScreen
    {
        public GameObject OptionsPanel;

        public CraftingTable Table;
        public IngredientShelf Shelf;

        public override void OnOptionsPressed()
        {
            OptionsPanel.SetActive(true);
        }

        public override void Options_OnReturnPressed()
        {
            OptionsPanel.SetActive(false);
        }

        public override void Options_OnQuitPressed()
        {

        }

        private void Start()
        {
            Table = GetComponentInChildren<CraftingTable>();
            Shelf = GetComponentInChildren<IngredientShelf>();
        }
    }
}
