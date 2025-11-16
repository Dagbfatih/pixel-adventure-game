using Godot;

public partial class Player : CharacterBody2D, ICharacterState, IHealth
{
	public Vector2 Direction { get; set; } = Vector2.Left;

	public override void _PhysicsProcess(double delta)
	{
		// Yalnızca yatay yönü kontrol ediyoruz.
		float inputX = 0.0f;

		if (Input.IsActionPressed("move_right"))
			inputX += 1;

		if (Input.IsActionPressed("move_left"))
			inputX -= 1;

		Direction = new Vector2(inputX, 0).Normalized();
	}
}
