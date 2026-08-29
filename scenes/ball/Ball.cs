using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 400.0f;

	[Export] public Vector2 InitialVelocity { get; set; } = new Vector2(150, -300);

	//Set difficulty (higher angle => harder)
	[Export] private float _maxBounceAngleDegrees = 60f;
	private float MaxBounceAngle => Mathf.DegToRad(_maxBounceAngleDegrees);

	public override void _Ready()
	{
		Velocity = InitialVelocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision is null) return;

		var collider = collision.GetCollider(); //colliding obj
		if (collider is not Paddle paddle)
		{
			Velocity = Velocity.Bounce(collision.GetNormal());
			return;
		}

		var paddleWidth = paddle.Width;

		var offset = (Position.X - paddle.Position.X) / (paddleWidth / 2f);
		offset = Mathf.Clamp(offset, -1f, 1f);

		var bounceAngle = offset * MaxBounceAngle;
		Velocity = new Godot.Vector2(
			Speed * Mathf.Sin(bounceAngle),
			-Speed * Mathf.Cos(bounceAngle)
		);
	}

}
