using System;
using System.Collections.Generic;

namespace VadyaRus.FSM {
    public class StateMachine<TContext> : IState<TContext> where TContext : IContext {
        private StateNode current;
        private readonly TContext context;
        private IState<TContext> initialState;


        private readonly Dictionary<Type, StateNode> nodes = new();

        private readonly HashSet<ITransition<TContext>> anyTransitions = new();

        public StateMachine(TContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual void OnEnter(TContext context) {
            if (initialState != null) {
                SetState(initialState);
            }
        }

        public void OnUpdate(TContext context) {
            var transition = GetTransition();
            if (transition != null) {
                ChangeState(transition.To);
            }

            current.State?.OnUpdate(context);
        }

        public void OnFixedUpdate(TContext context) {
            current.State?.OnFixedUpdate(context);
        }

        public void OnLateUpdate(TContext context) {
            current.State?.OnLateUpdate(context);
        }

        public virtual void OnExit(TContext context) {
            current.State?.OnExit(context);
            current = null;
        }

        public void SetState(IState<TContext> state) {
            current = nodes[state.GetType()];
            current.State?.OnEnter(context);
        }

        public IState<TContext> Current => current.State;

        void ChangeState(IState<TContext> state) {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit(context);
            current = nodes[state.GetType()];
            nextState?.OnEnter(context);
        }

        ITransition<TContext> GetTransition() {
            foreach (var transition in anyTransitions) {
                if (transition.Condition.Evaluate()) {
                    return transition;
                }
            }

            foreach (var transition in current.Transitions) {
                if (transition.Condition.Evaluate()) {
                    return transition;
                }
            }

            return null;
        }

        public void AddTransition(IState<TContext> from, IState<TContext> to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState<TContext> to, IPredicate condition) {
            anyTransitions.Add(new Transition<TContext>(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(IState<TContext> state) {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null) {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        class StateNode {
            public IState<TContext> State { get; }

            public HashSet<ITransition<TContext>> Transitions { get; }

            public StateNode(IState<TContext> state) {
                State = state;
                Transitions = new HashSet<ITransition<TContext>>();
            }

            public void AddTransition(IState<TContext> to, IPredicate condition) {
                Transitions.Add(new Transition<TContext>(to, condition));
            }
        }
    }
}