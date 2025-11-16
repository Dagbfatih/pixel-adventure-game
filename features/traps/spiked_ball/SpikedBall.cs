using System.Collections.Generic;
using System.Linq;
using Godot;

public enum RotationDirection
{
	CCW = -1,
	CW = 1
}

public partial class SpikedBall : Node2D, ICircularMovable
{
	[Export] public int SegmentCount = 4;
	[Export] public float SegmentSpacing = 16f;

	[Export(PropertyHint.Range, "0.1,10,0.1")]
	public float SwingDuration = 2f; // ⏱ Seconds for one full swing (left to right and back)

	[Export] public RotationDirection Direction = RotationDirection.CW;
	[Export] public float SwingAngleDegrees = 360f;

	private float centerAngle = 0f; // down direction
	private float swingAngleRad = 0f; // max angle in radians
	private float time = 0f;

	private Node2D _chainAnchor;
	private readonly List<CharacterBody2D> _chainParts = new();
	private CharacterBody2D _segment1;
	private CharacterBody2D _ball;
	private Area2D _hitbox;

	public override void _Ready()
	{
		GenerateChain();

		foreach (Node child in GetChildren())
		{
			if (child is CharacterBody2D body && child.Name != "ChainAnchor")
				_chainParts.Add(body);
		}

		centerAngle = Mathf.Pi / 2f;
		swingAngleRad = Mathf.DegToRad(SwingAngleDegrees / 2f);

		// Start phase: left or right
		time = (Direction == RotationDirection.CW) ? 0f : SwingDuration / 2f;

		_hitbox = _ball.GetNode<Area2D>("Hitbox");

		// 👇 Connect the signal
		_hitbox.BodyEntered += OnHitboxBodyEntered;
	}

	public override void _Process(double delta)
	{
		time += (float)delta;

		float angle;

		if (SwingAngleDegrees < 360f)
		{
			float frequency = 1f / SwingDuration; // Hz
			float omega = Mathf.Tau * frequency;
			angle = centerAngle + swingAngleRad * Mathf.Cos(omega * time);
		}
		else
		{
			float angularSpeed = Mathf.Tau / SwingDuration; // full turn in SwingDuration seconds
			angle = centerAngle + (int)Direction * angularSpeed * time;
			angle = Mathf.Wrap(angle, 0f, Mathf.Tau);
		}

		foreach (var part in _chainParts)
		{
			Vector2 offset = part.GlobalPosition - _chainAnchor.GlobalPosition;
			float distance = offset.Length();

			Vector2 newOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
			part.GlobalPosition = _chainAnchor.GlobalPosition + newOffset;
		}
	}

	private void GenerateChain()
	{
		_chainAnchor = GetNode<Node2D>("ChainAnchor");
		_segment1 = GetNode<CharacterBody2D>("Segment1");
		_ball = GetNode<CharacterBody2D>("Ball");

		Node2D prevNode = _chainAnchor;

		_segment1.Position = _chainAnchor.Position + new Vector2(0, SegmentSpacing);
		AddChild(CreatePinJointNode(prevNode, _segment1));
		prevNode = _segment1;

		for (int i = 1; i < SegmentCount; i++)
		{
			var newSegment = (CharacterBody2D)_segment1.Duplicate();
			newSegment.Name = $"Segment{i + 1}";
			newSegment.Position = prevNode.Position + new Vector2(0, SegmentSpacing);

			AddChild(newSegment);
			AddChild(CreatePinJointNode(prevNode, newSegment));
			prevNode = newSegment;
		}

		_ball.Position = prevNode.Position + new Vector2(0, SegmentSpacing);
		AddChild(CreatePinJointNode(prevNode, _ball));
	}

	private PinJoint2D CreatePinJointNode(Node2D a, Node2D b)
	{
		return new PinJoint2D
		{
			NodeA = a.GetPath(),
			NodeB = b.GetPath(),
			Position = ToLocal((a.GlobalPosition + b.GlobalPosition) / 2f),
			Softness = 0f,
		};
	}

	private void OnHitboxBodyEntered(Node2D body)
	{
		// 1. Check if the body is the player (you might use a specific C# class
		//    or check if it has the HealthComponent)

		// Assuming your player has a script/class called 'Player'
		if (body is Player player)
		{
			var fsm = player.GetChildren().OfType<FiniteStateMachine>().FirstOrDefault();
			if (fsm == null)
				return;

			// Knockback vector oluştur
			Vector2 knockbackVector = (player.GlobalPosition - GlobalPosition).Normalized() * 400f;
			int damage = 1;
			// FSM’e parametreli state değişikliği yap
			fsm.ChangeState("Hurt", new HurtStateParams(knockbackVector, damage));
		}
		// You can add additional checks here if the player has to be on the floor 
		// or pressing a button, but typical hazard damage is instant upon touch. 
		// The original code's `IsOnFloor()` and `Input.IsActionJustPressed()` 
		// checks are more appropriate for a player's attack or action, not a hazard's damage.
	}
}
