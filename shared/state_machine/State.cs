using Godot;

public partial class State : Node
{
	public FiniteStateMachine FSM { get; set; }
	protected CharacterBody2D Actor;
	protected AnimatedSprite2D Sprite;

	public string Key { get; protected set; }

	public override void _Ready()
	{
		FSM = GetParent() as FiniteStateMachine;
		Actor = FSM.GetOwner<CharacterBody2D>();
		Sprite = Actor.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public virtual void Enter(StateParams parameters = null) { }
	public virtual void Exit() { }
	public virtual void StatePhysicsProcess(double delta) { }
}
