using Animancer;

namespace Wcng
{
    public interface IState
    {
        public bool CanChangeState { get; }
        void OnInit<TState>(AnimancerComponent controller,StateMachine<TState> stateMachine,InputComponent inputComponent) where TState : class, IState;
        void OnEnter();
        void OnPhysicUpdate();
        void OnLogicUpdate();
        void OnExit();
    }
}
