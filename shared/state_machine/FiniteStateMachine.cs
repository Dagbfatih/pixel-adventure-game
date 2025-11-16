using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class FiniteStateMachine : Node
{
	[Export]
	private State DefaultState;

	private Dictionary<string, State> _states;

	public State CurrentState { get; private set; }
	public State PreviousState { get; private set; }

	public override void _Ready()
	{
		_states = new Dictionary<string, State>();
		GD.Print($"[FSM] Children count: {GetChildCount()}");
		foreach (var c in GetChildren())
			GD.Print($"[FSM] Child: {c.GetType().Name}  name:{c.Name}");

		foreach (var child in GetChildren().OfType<State>())
		{
			GD.Print($"[FSM] Found State node: {child.GetType().Name} key:{child.Key} name:{child.Name}");
			_states.Add(child.Key, child);
			child.FSM = this;
		}

		if (DefaultState == null)
		{
			GD.PrintErr("[FSM] DefaultState is NULL in Inspector!");
		}
		else
		{
			GD.Print("[FSM] DefaultState.Key: ", DefaultState.Key);
			CurrentState = DefaultState;
			CurrentState.Enter();
			GD.Print("[FSM] Entered DefaultState: ", CurrentState.Key);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.StatePhysicsProcess(delta);
	}

	public void ChangeState(string stateName, StateParams parameters = null)
	{
		if (!_states.TryGetValue(stateName, out State nextState) || CurrentState?.Key == stateName)
			return;

		PreviousState = CurrentState;
		CurrentState?.Exit();

		CurrentState = nextState;

		if (parameters != null)
			CurrentState.Enter(parameters); // parametreli Enter (generic değil)
		else
			CurrentState.Enter(); // parametresiz Enter
	}
}
