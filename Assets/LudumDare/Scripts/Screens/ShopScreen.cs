using UnityEngine;

namespace LD49.Screens
{
    using GUIElements;
    using Managers;

    public class ShopScreen : AbstractScreen
    {
        public CraftingTable Table;
        public IngredientShelf Shelf;

        private void Start()
        {
            Table = GetComponentInChildren<CraftingTable>();
            Shelf = GetComponentInChildren<IngredientShelf>();
        }

        private void Update()
        {

        }
    }
}
