using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LD49.Utilities
{
    public class StateMachine : MonoBehaviour
    {
        public State state = new State();

        [HideInInspector]
        public Enum lastState;

        protected float timeEnteredState;

        private Dictionary<Enum, Dictionary<string, Delegate>> _cache =
            new Dictionary<Enum, Dictionary<string, Delegate>>();

        protected Enum DelegateState
        {
            get {
                return state.currentState;
            }

            set {
                if ( state.currentState == value ) return;
                ChangingState();
                state.currentState = value;
                ConfigureCurrentState();
            }
        }

        private void ChangingState()
        {
            lastState = state.currentState;
            timeEnteredState = Time.time;
        }

        private void ConfigureCurrentState()
        {
            if ( state.exitState != null )
            {
                state.exitState();
            }

            state.DoUpdate = ConfigureDelegate<Action>("Update", DoNothing);
            state.DoFixedUpdate = ConfigureDelegate<Action>("FixedUpdate", DoNothing);
            state.DoLateUpdate = ConfigureDelegate<Action>("LateUpdate", DoNothing);
            state.enterState = ConfigureDelegate<Action>("EnterState", DoNothing);
            state.exitState = ConfigureDelegate<Action>("ExitState", DoNothing);

            if ( state.enterState != null )
            {
                state.enterState();
            }
        }

        private T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
        {
            Dictionary<string, Delegate> lookup;

            if ( !_cache.TryGetValue(state.currentState, out lookup) )
            {
                _cache[state.currentState] = lookup = new Dictionary<string, Delegate>();
            }

            Delegate returnValue;

            if ( !lookup.TryGetValue(methodRoot, out returnValue) )
            {

                MethodInfo mtd = GetType().GetMethod(
                    state.currentState.ToString() + "_" + methodRoot,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod
                );

                if ( mtd != null )
                {
                    returnValue = Delegate.CreateDelegate(typeof(T), this, mtd);
                }
                else
                {
                    returnValue = Default as Delegate;
                }

            }

            return returnValue as T;
        }


        private void Start() { }

        private void Update()
        {
            state.DoUpdate();
        }

        private void FixedUpdate()
        {
            state.DoFixedUpdate();
        }

        private void LateUpdate()
        {
            state.DoLateUpdate();
        }

        private static void DoNothing() { }

        public class State
        {
            public Action DoUpdate = DoNothing;
            public Action DoFixedUpdate = DoNothing;
            public Action DoLateUpdate = DoNothing;
            public Action enterState = DoNothing;
            public Action exitState = DoNothing;

            public Enum currentState;
        }
    }
}
