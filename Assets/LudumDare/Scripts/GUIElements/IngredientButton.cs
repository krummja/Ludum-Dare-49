using UnityEngine;
using UnityEngine.UI;

namespace LD49.GUIElements
{
    using Screens;
    using TMPro;
    using UnityEngine.EventSystems;

    public class IngredientButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Ingredient Ingredient;
        public CraftingTable Table;

        public bool IsSelected = false;
        public TextMeshProUGUI NameTooltip;

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
            NameTooltip = GetComponentInChildren<TextMeshProUGUI>();
            NameTooltip.gameObject.SetActive(false);

            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);

            Ingredient.IsUsed = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Mouse detected");
            NameTooltip.text = Ingredient.Name;
            NameTooltip.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            NameTooltip.text = "";
            NameTooltip.gameObject.SetActive(false);
        }
    }
}
