using Godot;

public partial class GravityComponent : Node
{
	[Export]
	public float Gravity = 1000.0f;

	[Export]
	public float MaxFallSpeed = 2000.0f;

	private CharacterBody2D _body;

	public override void _Ready()
	{
		_body = GetParent() as CharacterBody2D;
		if (_body == null)
		{
			GD.PushError("GravityComponent must be a direct child of a CharacterBody2D (e.g., Player).");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_body == null) return;

		Vector2 velocity = _body.Velocity;

		// ✅ Apply gravity
		velocity.Y += Gravity * (float)delta;

		// ✅ Clamp fall speed
		velocity.Y = Mathf.Min(velocity.Y, MaxFallSpeed);

		_body.Velocity = velocity;
		
		_body.MoveAndSlide();
	}
}
