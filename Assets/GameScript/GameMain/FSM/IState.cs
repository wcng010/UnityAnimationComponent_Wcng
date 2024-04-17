using Animancer;
using UnityEngine.Playables;

namespace Wcng.FSM
{
    public interface IState
    {
        public bool CanChangeState { get; }
        void OnInit<TState>(StateMachine<TState> stateMachine,AnimancerComponent controller, InputComponent inputComponent, PlayableDirector playableDirector)  where TState : class, IState;
        void OnEnter();
        void OnPhysicUpdate();
        void OnLogicUpdate();
        void OnExit();
    }
}
