using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LD49.GUIElements
{
    using System.Collections;
    using QTE;

    public enum ShopState
    {
        Selecting,
        Brewing,
        Result,
    }

    public class CraftingTable : SerializedMonoBehaviour
    {
        public IngredientShelf Shelf;
        public QTEManager QTEManager;

        public GameObject CenterButton;

        public List<TableButton> Buttons;

        public int[] InputSequence;
        public float[] InputTimings;

        public TableButton SlotA;
        public TableButton SlotB;
        public TableButton SlotC;
        public TableButton SlotD;
        public TableButton SlotE;

        public ShopState ShopState;

        private int _filled = 0;
        private bool _brewing = false;
        private int _lastIndex = 0;

        public void AddIngredientToSlot(Ingredient ingredient)
        {
            for ( int i = 0; i < 5; i++ )
            {
                TableButton button = Buttons[i];
                if ( !button.TableSlot.Ingredient )
                {
                    TableSlot slotData = button.TableSlot;

                    ingredient.IsUsed = true;
                    ingredient.UsedAt = i;
                    slotData.Ingredient = ingredient;
                    slotData.FilledIcon = ingredient.Icon;

                    _filled += 1;

                    break;
                }
            }
        }

        public void ClearSlot(int index)
        {
            TableButton button = Buttons[index];
            TableSlot slotData = button.TableSlot;

            slotData.Ingredient.IsUsed = false;
            slotData.Ingredient = null;
            slotData.FilledIcon = null;

            _filled -= 1;
        }

        public void CaptureInput(int buttonIndex)
        {
            QTEManager.CheckInput(buttonIndex);
        }

        private void Start()
        {
            Buttons = new List<TableButton>();
            Buttons.Add(SlotA);
            Buttons.Add(SlotB);
            Buttons.Add(SlotC);
            Buttons.Add(SlotD);
            Buttons.Add(SlotE);

            for ( int i = 0; i < 5; i++ )
            {
                Buttons[i].Index = i;
            }

            CenterButton.GetComponent<Button>().onClick.AddListener(StartBrewing);
        }

        private void Update()
        {
            switch ( ShopState )
            {
                case ShopState.Selecting:
                {
                    if ( _filled == 5 && !CenterButton.activeSelf )
                    {
                        CenterButton.SetActive(true);
                    }

                    if ( _filled < 5 && CenterButton.activeSelf )
                    {
                        CenterButton.SetActive(false);
                    }
                    break;
                }

                case ShopState.Brewing:
                {
                    CenterButton.SetActive(false);
                    Shelf.gameObject.SetActive(false);

                    if ( _brewing == false )
                    {
                        QTEEvent challenge = new QTEEvent();
                        challenge.Start();
                        QTEManager.StartEvent(challenge);

                        _brewing = true;
                    }
                    else
                    {
                        if ( _lastIndex == 10 )
                        {
                            _brewing = false;
                            ShopState = ShopState.Result;
                        }
                    }

                    break;
                }

                case ShopState.Result:
                {
                    break;
                }
            }
        }

        private void StartBrewing()
        {
            InputSequence = new int[10];
            InputTimings = new float[10];
            ShopState = ShopState.Brewing;
        }
    }
}
