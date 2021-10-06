using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LD49.GUIElements
{
    using Managers;
    using QTE;

    public enum ShopState
    {
        Selecting,
        Brewing,
        Result,
    }

    public enum Result
    {
        Playing,
        Win,
        Lose,
        GameEnd,
    }

    public class CraftingTable : SerializedMonoBehaviour
    {
        public IngredientShelf Shelf;
        public GameObject NeutralMochi;
        public GameObject FailureMochi;
        public QTEManager QTEManager;

        public GameObject CenterButton;
        public GameObject CompleteButton;

        public GameObject ChancesIndicator;
        public GameObject Chance1;
        public GameObject Chance2;
        public GameObject Chance3;

        public AudioSource SoundEffects;
        public AudioClip BubbleSound;
        public AudioClip ExplodeSound;

        public GameObject PotionSurface;
        public ParticleSystem BrewBubble;
        public ParticleSystem OverBubble;
        public ParticleSystem BigExplosion;
        public ParticleSystem SmokeExplosion;

        public List<TableButton> Buttons;

        public TableButton SlotA;
        public TableButton SlotB;
        public TableButton SlotC;
        public TableButton SlotD;
        public TableButton SlotE;

        public ShopState ShopState;

        public int ChancesInRound = 3;

        public Result Result = Result.Playing;

        private int _filled = 0;
        private bool _brewing = false;
        private int _lastIndex = 0;
        private bool reset = false;

        private QTEEvent _challenge;

        public void AddIngredientToSlot(Ingredient ingredient)
        {
            for ( int i = 0; i < 5; i++ )
            {
                TableButton button = Buttons[i];
                if ( !button.TableSlot.Ingredient )
                {
                    TableSlot slotData = button.TableSlot;
                    RectTransform t = button.gameObject.GetComponent<RectTransform>();

                    if ( ingredient.Name == "Sand" )
                    {
                        t.pivot = new Vector2(0.5f, 0f);
                        t.position = new Vector3(slotData.position.x, slotData.position.y - (75f / 2f), 0f);
                    }
                    else
                    {
                        t.pivot = new Vector2(0.5f, 0.5f);
                        t.position = new Vector3(slotData.position.x, slotData.position.y, 0f);
                    }

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

        public void ResetSlots()
        {
            _filled = 0;

            for ( int i = 0; i < 5; i++ )
            {
                TableButton button = Buttons[i];
                TableSlot slotData = button.TableSlot;

                slotData.Ingredient.IsUsed = false;
                slotData.Ingredient = null;
                slotData.FilledIcon = null;
            }
        }

        public void Explode()
        {
            SoundEffects.clip = ExplodeSound;
            SoundEffects.Play();

            BigExplosion.gameObject.SetActive(true);
            SmokeExplosion.gameObject.SetActive(true);
            BigExplosion.Play();
            SmokeExplosion.Play();
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
                Buttons[i].TableSlot.position = Buttons[i].gameObject.transform.position;
            }

            CenterButton.GetComponent<Button>().onClick.AddListener(StartBrewing);
            CompleteButton.GetComponent<Button>().onClick.AddListener(NextStage);

            SoundEffects = GetComponent<AudioSource>();
        }

        private void Update()
        {
            switch ( ShopState )
            {
                case ShopState.Selecting:
                {
                    Shelf.gameObject.SetActive(true);

                    if ( reset )
                    {
                        CenterButton.SetActive(false);
                        ResetSlots();
                        reset = false;
                    }

                    if ( _filled == 5 && !CenterButton.activeSelf )
                    {
                        switch ( GameManager.Instance.StoryStage )
                        {
                            case StoryStage.FirstPotion:
                            {
                                if ( ValidateIngredients(1))
                                {
                                    CenterButton.SetActive(true);
                                };
                                break;
                            }

                            case StoryStage.SecondPotion:
                            {
                                if ( ValidateIngredients(2))
                                {
                                    CenterButton.SetActive(true);
                                };
                                break;
                            }

                            case StoryStage.ThirdPotion:
                            {
                                if ( ValidateIngredients(3))
                                {
                                    CenterButton.SetActive(true);
                                };
                                break;
                            }
                        }
                    }

                    if ( _filled < 5 && CenterButton.activeSelf )
                    {
                        CenterButton.SetActive(false);
                    }
                    break;
                }

                case ShopState.Brewing:
                {
                    Shelf.gameObject.SetActive(false);
                    CenterButton.SetActive(false);
                    ChancesIndicator.SetActive(true);

                    switch ( ChancesInRound )
                    {
                        case 3:
                            break;
                        case 2:
                            Chance1.SetActive(false);
                            break;
                        case 1:
                            Chance2.SetActive(false);
                            break;
                        default:
                            Chance3.SetActive(false);
                            break;
                    }

                    if ( _brewing == false )
                    {
                        QTEEvent challenge = new QTEEvent();
                        _challenge = challenge;

                        // Use these events to handle the effects prior to transition back to Story mode.

                        UnityEvent failEvent = new UnityEvent();
                        failEvent.AddListener(BrewingFailure);

                        UnityEvent successEvent = new UnityEvent();
                        successEvent.AddListener(BrewingSuccess);

                        UnityEvent brewingEndedEvent = new UnityEvent();
                        brewingEndedEvent.AddListener(BrewingEnd);

                        challenge.onFail = failEvent;
                        challenge.onSuccess = successEvent;
                        challenge.onEnd = brewingEndedEvent;

                        challenge.Start();
                        QTEManager.StartEvent(challenge);

                        _brewing = true;
                    }

                    break;
                }

                case ShopState.Result:
                {
                    if ( Result == Result.Playing )
                    {
                        ShopState = ShopState.Selecting;
                        reset = true;
                        _brewing = false;
                    }

                    if ( Result == Result.Win )
                    {
                        switch ( GameManager.Instance.StoryStage )
                        {
                            case StoryStage.FirstPotion:
                                Result = Result.Playing;
                                GameManager.Instance.StoryStage = StoryStage.SecondPotion;
                                break;
                            case StoryStage.SecondPotion:
                                Result = Result.Playing;
                                GameManager.Instance.StoryStage = StoryStage.ThirdPotion;
                                break;
                            case StoryStage.ThirdPotion:
                                Result = Result.GameEnd;
                                GameManager.Instance.StoryStage = StoryStage.Win;
                                break;
                        }
                    }

                    if ( Result == Result.Lose )
                    {
                        Result = Result.GameEnd;
                        GameManager.Instance.StoryStage = StoryStage.Lose;
                    }

                    break;
                }
            }
        }

        private void StartBrewing()
        {
            ShopState = ShopState.Brewing;
            ChancesInRound = 3;
        }

        private void NextStage()
        {
            Result = Result.Playing;
            CompleteButton.SetActive(false);
            QTEManager.Nice.gameObject.SetActive(false);
            QTEManager.Bad.gameObject.SetActive(false);
            QTEManager.Missed.gameObject.SetActive(false);
            GameManager.Instance.TransitionToState(GameState.Story);
        }

        private void BrewingSuccess()
        {
            Result = Result.Win;
            CompleteButton.SetActive(true);
            Debug.Log("Nice Job!!");
        }

        private void BrewingFailure()
        {
            Result = Result.Lose;

            StartCoroutine(FailureEndingRoutine());
            Explode();

            NeutralMochi.GetComponent<SpriteRenderer>().enabled = false;
            FailureMochi.GetComponent<SpriteRenderer>().enabled = true;
        }

        private void BrewingEnd()
        {
            Debug.Log("Brewing stage ended");
        }

        private IEnumerator FailureEndingRoutine()
        {
            Debug.Log("Starting Failure Coroutine");
            yield return new WaitForSecondsRealtime(5);
            GameManager.Instance.TransitionToState(GameState.Transition);
        }

        private bool ValidateIngredients(int stage)
        {
            Potion potion;

            switch ( stage )
            {
                default:
                    potion = new FirstPotion();
                    break;
                case 2:
                    potion = new SecondPotion();
                    break;
                case 3:
                    potion = new ThirdPotion();
                    break;
            }

            for ( int i = 0; i < 5; i++ )
            {
                TableSlot slot = Buttons[i].TableSlot;
                potion.Check(slot.Ingredient);
            }

            return potion.Validate();
        }

        private class FirstPotion : Potion
        {
            public override bool Check(Ingredient ingredient)
            {
                if ( IngredientCheck == null )
                {
                    IngredientCheck = new bool[] { false, false, false, false, false };
                }

                switch ( ingredient.Name )
                {
                    case "Water":
                        IngredientCheck[0] = true;
                        break;
                    case "Chamomile Tea":
                        IngredientCheck[1] = true;
                        break;
                    case "Honey":
                        IngredientCheck[2] = true;
                        break;
                    case "Warm Milk":
                        IngredientCheck[3] = true;
                        break;
                    case "Sand":
                        IngredientCheck[4] = true;
                        break;
                }

                return false;
            }
        }

        private class SecondPotion : Potion
        {
            public override bool Check(Ingredient ingredient)
            {
                if ( IngredientCheck == null )
                {
                    IngredientCheck = new bool[] { false, false, false, false, false };
                }

                switch ( ingredient.Name )
                {
                    case "Chocolate":
                        IngredientCheck[0] = true;
                        break;
                    case "Flower Petals":
                        IngredientCheck[1] = true;
                        break;
                    case "Feather":
                        IngredientCheck[2] = true;
                        break;
                    case "Wine":
                        IngredientCheck[3] = true;
                        break;
                    case "Poem":
                        IngredientCheck[4] = true;
                        break;
                }

                return false;
            }

            public bool Validate()
            {
                if ( IngredientCheck.All(x => x) ) return true;
                return false;
            }
        }

        private class ThirdPotion : Potion
        {
            public override bool Check(Ingredient ingredient)
            {
                if ( IngredientCheck == null )
                {
                    IngredientCheck = new bool[] { false, false, false, false, false };
                }

                switch ( ingredient.Name )
                {
                    case "Pieces of a Mirror":
                        IngredientCheck[0] = true;
                        break;
                    case "Caterpillar Cocoon":
                        IngredientCheck[1] = true;
                        break;
                    case "Snake Skin":
                        IngredientCheck[2] = true;
                        break;
                    case "Bottle of Liquid Moonlight":
                        IngredientCheck[3] = true;
                        break;
                    case "Whole Egg":
                        IngredientCheck[4] = true;
                        break;
                }

                return false;
            }
        }

        private abstract class Potion
        {
            protected bool[] IngredientCheck = new bool[] { false, false, false, false, false };

            public abstract bool Check(Ingredient ingredient);

            public bool Validate()
            {
                if ( IngredientCheck.All(x => x) ) return true;
                return false;
            }
        }

        [Button("Skip")]
        private void DebugSkip()
        {
            QTEManager.SequenceIndex = 10;
        }
    }
}
