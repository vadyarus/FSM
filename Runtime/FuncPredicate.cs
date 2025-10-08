using System;

namespace VadyaRus.FSM {
    public class FuncPredicate : IPredicate { 
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> predicate) { 
            this.func = predicate;
        }

        public bool Evaluate() => func.Invoke();
    }
}