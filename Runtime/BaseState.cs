namespace VadyaRus.FSM {
    public abstract class BaseState<TContext> : IState<TContext> where TContext : IContext {
        public virtual void OnEnter(TContext context) { }
        public virtual void OnUpdate(TContext context) { }
        public virtual void OnFixedUpdate(TContext context) { }
        public virtual void OnLateUpdate(TContext context) { }
        public virtual void OnExit(TContext context) { }
    }
}