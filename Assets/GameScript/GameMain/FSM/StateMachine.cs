using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.Playables;

namespace Wcng.FSM
{
    public abstract class StateMachine<TState> : IStateMachine where TState : class, IState
    {
        
        private TState _OriginalState;
        public TState OriginalState 
        {
            get
            {
                if (_OriginalState == null) return null;
                if (!_StateList.Contains(_OriginalState))
                {
                    _OriginalState.OnInit(this,AnimancerComponent,InputComponent,PlayableDirector);
                    _StateList.Add(_OriginalState);
                }
                return _OriginalState;
            }
            set => _OriginalState = value;
        }
        object IStateMachine.OriginalState => OriginalState;

        
        private TState _CurrentState;
        public TState CurrentState
        {
            get
            {
                if (_CurrentState == null) return null;
                if (!_StateList.Contains(_CurrentState))
                {
                    _CurrentState.OnInit(this,AnimancerComponent,InputComponent,PlayableDirector);
                    _StateList.Add(_CurrentState);
                }
                return _CurrentState;
            }
            set => _CurrentState = value;
        }
        object IStateMachine.CurrentState => CurrentState;
        
        private TState _PreviousState;
        public TState PreviousState
        {
            get
            {
                if (_PreviousState == null) return null;
                if (!_StateList.Contains(_PreviousState))
                {
                    _PreviousState.OnInit(this,AnimancerComponent,InputComponent,PlayableDirector);
                    _StateList.Add(_PreviousState);
                }
                return _PreviousState;
            }
            set => _PreviousState = value;
        }
        object IStateMachine.PreviousState => PreviousState;
        
        protected InputComponent InputComponent;
        protected AnimancerComponent AnimancerComponent;
        protected PlayableDirector PlayableDirector;
        
        private readonly List<IState> _StateList = new List<IState>();
        
        public void ChangeState(object state)
        {
            TState stateTemp = state as TState;
            if (!_StateList.Contains(stateTemp))
            {
                stateTemp?.OnInit(this,AnimancerComponent,InputComponent,PlayableDirector);
                _StateList.Add(stateTemp);
            }
            if (CurrentState == null)
            {
                stateTemp?.OnEnter();
                CurrentState = stateTemp;
            }
            else if (stateTemp != CurrentState && CurrentState.CanChangeState)
            {
                CurrentState?.OnExit(); 
                stateTemp?.OnEnter();
                PreviousState = CurrentState; 
                CurrentState = stateTemp;
            }
        }

        public void LogicUpdate()
        {
            CurrentState.OnLogicUpdate();
        }

        public void FixedUpdate()
        {
            CurrentState.OnPhysicUpdate();
        }
        
        public virtual void BackLastState()
        {
            CurrentState?.OnExit();
            PreviousState?.OnEnter();
            (PreviousState, CurrentState) = (CurrentState, PreviousState);
        }

        public virtual void BackOriginalState()
        {
            CurrentState?.OnExit();
            OriginalState?.OnEnter();
            PreviousState = _CurrentState;
        }
        
    }
}
