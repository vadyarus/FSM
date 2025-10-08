namespace VadyaRus.FSM {
    public interface ITransition<TContext> where TContext : IContext {
        IState<TContext> To { get; }
        IPredicate Condition { get; }
    }
}