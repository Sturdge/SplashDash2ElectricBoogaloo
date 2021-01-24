namespace StateMachine
{

    /// <summary>
    /// Class to contain the definition and logic of the StateMachine classe
    /// </summary>
    /// <typeparam name="T">The object that will use this state machine</typeparam>
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        private readonly T parent;

        /// <summary>
        /// Constructor to intialise the state machine
        /// </summary>
        /// <param name="newParent">The object the state machine is on</param>
        public StateMachine(T newParent)
        {
            parent = newParent;
            CurrentState = null;
        }

        /// <summary>
        /// Method for changing an object's state
        /// </summary>
        /// <param name="newState">The state to change to</param>
        public void ChangeState(State<T> newState)
        {
            CurrentState?.ExitState(parent);
            CurrentState = newState;
            CurrentState.EnterState(parent);
        }

    }

    /// <summary>
    /// Class to contain the definition and logic of the State classe
    /// </summary>
    /// <typeparam name="T">The object that will use this state</typeparam>
    public abstract class State<T>
    {
        public abstract void EnterState(T parent);
        public abstract void UpdateState(T parent);
        public abstract void ExitState(T parent);
    }
}