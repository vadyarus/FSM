namespace VadyaRus.FSM {
    public class Transition<TContext> : ITransition<TContext> where TContext : IContext {
        public IState<TContext> To { get; }

        public IPredicate Condition { get; }

        public Transition(IState<TContext> to, IPredicate condition) {
            To = to;
            Condition = condition;
        }
    }
}