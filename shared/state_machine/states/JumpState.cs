using Godot;

public partial class JumpState : State
{
	public JumpState()
	{
		this.Key = "Jump";
	}

	[Export] public float InitialJumpVelocity = -350f;
	[Export] public float JumpAcceleration = -6500f;
	[Export] public float MaxJumpVelocity = -350f;
	[Export] public float MaxJumpTime = 0.15f;
	[Export] public float AirControlSpeed = 200f;

	private float _elapsed = 0f;
	private bool _jumping = true;

	public override void Enter()
	{
		_elapsed = 0f;
		_jumping = true;

		if (Actor is CharacterBody2D body)
		{
			var v = body.Velocity;
			v.Y = InitialJumpVelocity;
			body.Velocity = v;
		}
		
		Sprite?.Play("jump");
	}

	public override void StatePhysicsProcess(double delta)
	{
		if (Actor is not ICharacterState ch || Actor is not CharacterBody2D body)
			return;

		_elapsed += (float)delta;
		var v = body.Velocity;
		v.X = ch.Direction.X * AirControlSpeed;

		if (_jumping)
		{
			v.Y += JumpAcceleration * (float)delta;
			if (v.Y < MaxJumpVelocity || _elapsed >= MaxJumpTime || !Input.IsActionPressed("jump"))
				_jumping = false;
		}

		body.Velocity = v;

		if (v.Y > 0)
			FSM.ChangeState("Fall");
	}
}
