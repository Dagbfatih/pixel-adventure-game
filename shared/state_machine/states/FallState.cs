using Godot;

public partial class FallState : State
{
	public FallState()
	{
		this.Key = "Fall";
	}

	[Export] public float AirControlSpeed = 200.0f;

	public override void Enter()
	{
		Sprite?.Play("fall");
	}

	public override void StatePhysicsProcess(double delta)
	{
		if (Actor is not ICharacterState character || Actor is not CharacterBody2D body)
			return;

		// 🪂 Havada yön kontrolü
		Vector2 velocity = body.Velocity;
		velocity.X = character.Direction.X * AirControlSpeed;
		body.Velocity = velocity;

		// ✅ Yere basınca duruma göre geçiş
		if (body.IsOnFloor())
		{
			if (character.Direction == Vector2.Zero)
				FSM.ChangeState("Idle");
			else
				FSM.ChangeState("Run");
		}
	}
}
