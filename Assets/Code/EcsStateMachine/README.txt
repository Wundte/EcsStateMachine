# ECS State Machine

Graph-based state machine framework built on top of **LeoEcsLite**.

The package allows configuring ECS states visually using Unity Graph Toolkit, automatically generating required identifiers and factories, and controlling system execution through state transitions.

## Features

- Visual state machine editor based on Unity Graph Toolkit
- ECS-oriented architecture
- LeoEcsLite integration
- Feature-based system activation
- State enter / exit systems
- Runtime graph generation
- Automatic enum and factory generation
- Stable identifiers for states and systems
- Unity serialization friendly graph format
- Dependency injection support through LeoEcsLite DI

## Architecture Overview

The state machine consists of three main parts:

```text
Editor Graph
      |
      v
Graph Importer
      |
      v
Runtime Graph
      |
      v
ECS Initialization
      |
      v
Runtime State Service
```

## Core Concepts

### State

A state represents an ECS gameplay state.

Examples:

- Main Menu
- Battle
- Shop
- Reward Screen
- Loading

Each state can contain:

- Enter systems
- Exit systems
- Active features
- Allowed transitions

Example:

```text
Battle State

On Enter:
    SpawnSystem
    SetupBattleSystem

Features:
    MovementFeature
    CombatFeature

On Exit:
    CleanupBattleSystem
```

### Feature

A feature is a group of ECS systems that are activated together after a state becomes active.

Example:

```csharp
public sealed class CombatFeature : EcsFeature
{
    public override IEcsSystem[] GetSystems()
    {
        return new IEcsSystem[]
        {
            new AttackSystem(),
            new DamageSystem(),
            new DeathSystem()
        };
    }
}
```

When activated, all systems inside the feature are added to the ECS execution group.

Example:

```text
CombatFeature

    AttackSystem
    DamageSystem
    DeathSystem
```

### State Change Systems

State change systems are executed during transitions.

Two types exist:

**Enter Systems**

Executed when entering a state.

Example:

```text
BattleState Enter

SpawnEnemiesSystem
InitializeBattleSystem
```

**Exit Systems**

Executed before leaving a state.

Example:

```text
BattleState Exit

SaveResultSystem
CleanupBattleSystem
```

Implementation:

```csharp
public sealed class PrepareBattleSystem : IEcsStateChangeSystem
{
    public void Run()
    {
        // State initialization logic
    }
}
```

## Runtime Flow

### Initialization

When the state machine starts:

```text
1. Load Runtime Graph

2. Create ECS system groups

3. Read states from graph

4. Add state change systems

5. Create feature groups

6. Inject dependencies

7. Initialize ECS systems

8. Start ECS loop
```

### State Transition Flow

When changing state:

```text
Old State

    |
    v

Disable old features

    |
    v

Run OnStateExit Systems

    |
    v

Wait one frame

    |
    v

Run OnStateEnter Systems

    |
    v

Enable new features

    |
    v

New State
```

## Code Generation

The package automatically generates:

### State IDs

```text
EcsStatesIds
```

Example:

```csharp
public enum EcsStatesIds
{
    None,

    Menu = 123456,
    Battle = 789123
}
```

### Feature IDs

```text
EcsFeatureIds
```

Used for selecting features inside graph nodes.

### State Change System IDs

```text
EcsStateChangeSystemsIds
```

Used for selecting enter/exit systems.

### Factories

Generated automatically:

```text
EcsFeatureFactory

EcsStateChangeSystemsFactory
```

They allow creating ECS objects from generated identifiers.

## Stable IDs

The package does not use enum indexes for runtime references.

Instead, IDs are generated from names:

```text
State Name
      |
      v
StableId
      |
      v
Integer ID
```

Example:

```text
Battle
   |
   v
123456789
```

This allows:

- Persistent references
- Graph serialization
- No dependency on enum ordering

## Runtime Data

Runtime graph does not store direct references between states.

Instead:

```csharp
public int DefaultNextState;

public List<int> PossibleNextStates;
```

contain stable state identifiers.

This avoids Unity serialization depth limitations and allows cyclic graphs:

```text
State A
  |
  v
State B
  |
  v
State A
```

## Creating Custom Feature

Create a class derived from `EcsFeature`:

```csharp
public sealed class MovementFeature : EcsFeature
{
    public override IEcsSystem[] GetSystems()
    {
        return new IEcsSystem[]
        {
            new MovementSystem(),
            new RotationSystem()
        };
    }
}
```

After code generation the feature becomes available inside graph editor.

## Creating State Change System

Implement:

```csharp
public sealed class EnterBattleSystem : IEcsStateChangeSystem
{
    public void Run()
    {
        // Execute logic on state change
    }
}
```

The system becomes available after regeneration.

## Design Decisions

### Why dictionary instead of references?

Unity has serialization depth limitations.

Using:

```csharp
Dictionary<int, RuntimeStateNode>
```

allows storing cyclic graphs safely.

### Why Features?

Features separate gameplay modules from states.

Instead of:

```text
Battle State

50 systems
```

we have:

```text
Battle State

Combat Feature
Movement Feature
AI Feature
```

This improves scalability and readability.

### Why separate State Change Systems?

State transitions are different from regular ECS update logic.

They execute only once during transition:

```text
Enter State
    |
    v
Execute initialization logic
```

instead of every frame.

## Requirements

- Unity 6+
- LeoEcsLite
- LeoEcsLite.ExtendedSystems
- LeoEcsLite.Di
- Unity Graph Toolkit
- Odin Inspector
- UniTask

## License

MIT License