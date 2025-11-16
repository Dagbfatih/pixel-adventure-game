using Godot;

public partial class IdleState : State
{
	public IdleState()
	{
		this.Key = "Idle";
	}

	public override void Enter(StateParams parameters = null)
	{
		Sprite?.Play("idle");
	}

	public override void StatePhysicsProcess(double delta)
	{

		if (Actor is not ICharacterState character)
			return;

		if (Actor is not CharacterBody2D body)
		{
			GD.PrintErr("IdleState requires Actor to be CharacterBody2D.");
			return;
		}

		GD.Print("IN IDLEEEEEEE", Actor.Velocity);


		// Eğer hareket varsa RunState'e geç
		if (character.Direction != Vector2.Zero)
		{
			FSM.ChangeState("Run");
			return;
		}

		if (!body.IsOnFloor())
		{
			GD.Print("FALLLLLLLLLL FROM IDLE");
			FSM.ChangeState("Fall");
			return;
		}

		if (Input.IsActionJustPressed("jump") && body.IsOnFloor())
		{
			FSM.ChangeState("Jump");
			return;
		}

		// Karakter sabit: Hareket yok, düşme yok
		Vector2 velocity = body.Velocity;
		velocity.X = Mathf.MoveToward(velocity.X, 0, 50); // X ekseninde yavaşlama
		body.Velocity = velocity;
		body.MoveAndSlide();
	}
}
