using Animancer;

namespace Wcng.FSM
{
    public interface IStateMachine
    {
        object OriginalState { get; }
        object CurrentState { get; }
        object PreviousState { get; }
        void ChangeState(object state);
        void BackLastState();
    }
}
