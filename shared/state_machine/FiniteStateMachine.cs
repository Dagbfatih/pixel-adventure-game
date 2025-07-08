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
		_states = [];
		foreach (var child in GetChildren().OfType<State>())
		{
			_states.Add(child.Key, child);
			child.FSM = this;
		}

		if (DefaultState != null)
		{
			CurrentState = DefaultState;
			CurrentState.Enter();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.StatePhysicsProcess(delta);
	}

	// Isim 'ChangeState' olarak güncellendi ve 'PreviousState' mantığı eklendi.
	public void ChangeState(string stateName)
	{
		if (!_states.TryGetValue(stateName, out State value) || CurrentState?.Name == stateName)
		{
			return;
		}

		PreviousState = CurrentState; // Mevcut state'i bir önceki olarak ayarla
		CurrentState?.Exit();

		CurrentState = value;
		CurrentState.Enter();
	}
}
