using Godot;

public partial class RunState : State
{
	[Export]
	public float Speed { get; set; } = 200.0f;

	public RunState()
	{
		this.Key = "Run";
	}

	public override void Enter()
	{
		Sprite?.Play("run");

		// Karakter sola gidiyorsa flip et, sağa gidiyorsa normal
		if (Actor is ICharacterState character)
		{
			Sprite.FlipH = character.Direction.X < 0;
		}
	}

	public override void StatePhysicsProcess(double delta)
	{
		// ✅ Make sure Actor implements ICharacterState
		if (Actor is not ICharacterState character)
			return;

		// ✅ Cast Actor to CharacterBody2D to access movement
		if (Actor is not CharacterBody2D body)
		{
			GD.PrintErr("RunState requires Actor to be CharacterBody2D.");
			return;
		}


		if (character.Direction == Vector2.Zero)
		{
			FSM.ChangeState("Idle");
			return;
		}

		if (Input.IsActionJustPressed("jump") && body.IsOnFloor())
		{
			FSM.ChangeState("Jump");
			return;
		}

		if (!body.IsOnFloor())
		{
			GD.Print($"FALL FROM RUN STATE: {character.Direction}");

			FSM.ChangeState("Fall");
			return;
		}


		GD.Print($"RUNNNNNNNN: {character.Direction}");

		// ✅ Movement code
		Vector2 velocity = body.Velocity;
		velocity.X = character.Direction.X * Speed;

		body.Velocity = velocity;
		body.MoveAndSlide();
	}
}
