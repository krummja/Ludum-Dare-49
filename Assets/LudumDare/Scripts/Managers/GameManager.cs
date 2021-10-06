using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Managers
{
    using System.Collections;
    using GUIElements;
    using Screens;
    using TMPro;
    using UnityEngine.UI;

    public enum GameState
    {
        Menu,
        Intro,
        Story,
        Shop,
        Ending,
        Transition,
    }

    public enum StoryStage
    {
        Intro,
        FirstPotion,
        SecondPotion,
        ThirdPotion,
        Win,
        Lose,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public AbstractScreen MainMenu;
        public AbstractScreen Intro;
        public AbstractScreen Story;
        public AbstractScreen Shop;
        public AbstractScreen Ending;

        public GameObject FadePanel;

        public StoryStage StoryStage;

        public AbstractScreen CurrentScreen { get; private set; }

        public GameState CurrentState { get; private set; }

        private bool _transition;

        public void TransitionToState(GameState newState)
        {
            GameState tmpInitialState = CurrentState;
            OnStateExit(tmpInitialState, newState);
            CurrentState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        public void OnStateEnter(GameState state, GameState fromState)
        {
            switch ( state )
            {
                case GameState.Menu:
                {
                    CurrentScreen = MainMenu;
                    MainMenu.gameObject.SetActive(true);
                    FadePanel.SetActive(false);
                    break;
                }

                case GameState.Intro:
                {
                    CurrentScreen = Story;
                    Story.gameObject.SetActive(true);
                    FadePanel.SetActive(false);
                    Story.GetComponent<StoryScreen>().Play();
                    break;
                }

                case GameState.Story:
                {
                    CurrentScreen = Story;
                    Story.gameObject.SetActive(true);
                    FadePanel.SetActive(false);
                    Story.GetComponent<StoryScreen>().Play();
                    break;
                }

                case GameState.Shop:
                {
                    CameraManager.Instance.SwitchCamera();
                    CurrentScreen = Shop;
                    Shop.gameObject.SetActive(true);
                    FadePanel.SetActive(false);
                    break;
                }

                case GameState.Ending:
                {
                    Debug.Log("Ending!");
                    CurrentScreen = Ending;
                    Ending.gameObject.SetActive(true);
                    FadePanel.SetActive(false);
                    break;
                }

                case GameState.Transition:
                {
                    FadePanel.SetActive(true);

                    switch ( fromState )
                    {
                        case GameState.Transition:
                            if ( StoryStage == StoryStage.Lose || StoryStage == StoryStage.Win)
                            {
                                CameraManager.Instance.SwitchCamera();
                            }

                            StartCoroutine(FadeTransition(true, 0f, GameState.Story));
                            break;

                        case GameState.Intro:
                            StartCoroutine(FadeTransition(false, 2f, GameState.Transition));
                            FadePanel.GetComponentInChildren<TextMeshProUGUI>().text = "A few months ago...";
                            break;

                        case GameState.Shop:
                            Shop.gameObject.SetActive(false);

                            if ( StoryStage == StoryStage.Win )
                            {
                                StartCoroutine(FadeTransition(false, 2f, GameState.Transition));
                                FadePanel.GetComponentInChildren<TextMeshProUGUI>().text = "The next day...";
                            }
                            else if ( StoryStage == StoryStage.Lose )
                            {
                                StartCoroutine(FadeTransition(false, 2f, GameState.Transition));
                                FadePanel.GetComponentInChildren<TextMeshProUGUI>().text = "The next day...";
                            }
                            break;
                    }

                    break;
                }
            }
        }

        public void OnStateExit(GameState state, GameState toState)
        {
            switch ( state )
            {
                case GameState.Menu:
                {
                    MainMenu.gameObject.SetActive(false);
                    // Intro.gameObject.SetActive(false);
                    Story.gameObject.SetActive(false);
                    Shop.gameObject.SetActive(false);
                    // Ending.gameObject.SetActive(false);
                    break;
                }

                case GameState.Intro:
                case GameState.Story:
                {
                    Story.gameObject.SetActive(false);
                    Story.GetComponent<StoryScreen>().Stop();
                    break;
                }

                case GameState.Shop:
                {
                    if (toState != GameState.Transition)
                    {
                        CameraManager.Instance.SwitchCamera();
                        Shop.gameObject.SetActive(false);
                    }
                    else
                    {
                        Shop.gameObject.SetActive(false);
                    }

                    break;
                }

                case GameState.Ending:
                {
                    Shop.gameObject.SetActive(false);
                    break;
                }
            }
        }

        public void QuitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void Awake()
        {
            if ( Instance != null )
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            CurrentState = GameState.Menu;
            StoryStage = StoryStage.Intro;
            TransitionToState(GameState.Menu);
        }

        private IEnumerator FadeTransition(bool fadeAway, float holdDuration, GameState nextState)
        {
            Image img = FadePanel.GetComponent<Image>();

            if ( fadeAway )
            {
                for ( float i = 1; i >= 0; i -= Time.deltaTime )
                {
                    img.color = new Color(0, 0, 0, i);
                    yield return null;
                }
            }
            else
            {
                for ( float i = 0; i <= 1; i += Time.deltaTime )
                {
                    img.color = new Color(0, 0, 0, i);
                    yield return null;
                }
            }

            if ( holdDuration > 0 )
            {
                yield return new WaitForSecondsRealtime(holdDuration);
            }

            TransitionToState(nextState);
        }
    }
}
