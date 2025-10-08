namespace VadyaRus.FSM {
    public interface IState<TContext> where TContext : IContext {
        void OnEnter(TContext context);
        void OnUpdate(TContext context);
        void OnFixedUpdate(TContext context);
        void OnLateUpdate(TContext context);
        void OnExit(TContext context);
    }
}