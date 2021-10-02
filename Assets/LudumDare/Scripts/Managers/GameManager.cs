using UnityEngine;
using Sirenix.OdinInspector;

namespace LD49.Managers
{
    public enum GameState
    {
        Menu,
        Intro,
        Story,
        Shop,
        Ending,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameObject MainMenu;
        public GameObject Story;
        public GameObject Shop;

        public GameState CurrentState { get; private set; }

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
                    break;
                }

                case GameState.Intro:
                {
                    break;
                }

                case GameState.Story:
                {
                    break;
                }

                case GameState.Shop:
                {
                    break;
                }

                case GameState.Ending:
                {
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
                    break;
                }

                case GameState.Intro:
                {
                    break;
                }

                case GameState.Story:
                {
                    break;
                }

                case GameState.Shop:
                {
                    break;
                }

                case GameState.Ending:
                {
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
        }

        private void Update()
        {
            switch ( CurrentState )
            {
                case GameState.Menu:
                {
                    break;
                }

                case GameState.Intro:
                {
                    break;
                }

                case GameState.Story:
                {
                    break;
                }

                case GameState.Shop:
                {
                    break;
                }

                case GameState.Ending:
                {
                    break;
                }
            }
        }
    }
}
