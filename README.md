# VadyaRus FSM

A flexible and easy-to-use Finite State Machine (FSM) for Unity. This package provides a simple yet powerful way to manage states and transitions in your game logic.

## 🚀 Features

* **Generic and Type-Safe:** Works with any class or struct that implements the `IContext` interface.
* **Simple State Management:** Easily define states, transitions, and conditions.
* **"Any State" Transitions:** Create transitions that can be triggered from any state.
* **Clean and Modular:** Encourages clean separation of logic for different states.

## 📦 Installation

You can add this package to your Unity project using the Package Manager.

1.  In the Unity Editor, go to **Window > Package Manager**.
2.  Click the **"+"** icon in the top-left corner.
3.  Select **"Add package from git URL..."**.
4.  Enter the following URL:

    ```
    https://github.com/vadyarus/fsm.git
    ```

5.  Click **Add**.

##  Usage

### 1. Create a Context

The context is a class that holds any data your states need to access.

```csharp
using VadyaRus.FSM;

public class EnemyContext : IContext {
    public float Health { get; set; }
    public Transform Target { get; set; }
}
```

### 2. Create States
Create classes for each state that inherit from `BaseState<TContext>`.
```csharp
using UnityEngine;
using VadyaRus.FSM;

public class IdleState : BaseState<EnemyContext> {
    public override void OnEnter(EnemyContext context) {
        Debug.Log("Entering Idle State");
    }
}

public class ChaseState : BaseState<EnemyContext> {
    public override void OnUpdate(EnemyContext context) {
        // Logic to chase the target
    }
}
```

### 3. Create and Configure the State Machine
```csharp
using UnityEngine;
using VadyaRus.FSM;

public class EnemyAI : MonoBehaviour {
    private StateMachine<EnemyContext> stateMachine;
    private EnemyContext context;

    void Start() {
        context = new EnemyContext { Health = 100f, Target = GameObject.FindWithTag("Player").transform };
        stateMachine = new StateMachine<EnemyContext>(context);

        // Define states
        var idle = new IdleState();
        var chase = new ChaseState();
        var flee = new FleeState();

        // Define transitions
        stateMachine.AddTransition(idle, chase, new FuncPredicate(() => Vector3.Distance(transform.position, context.Target.position) < 5f));
        stateMachine.AddTransition(chase, flee, new FuncPredicate(() => context.Health < 20f));

        // Set the initial state
        stateMachine.SetState(idle);
    }

    void Update() {
        stateMachine.OnUpdate(context);
    }
}
```

📜 License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/vadyarus/FSM/tree/main?tab=MIT-1-ov-file) file for details.