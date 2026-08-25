using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export]
	public float Speed = 400.0f;

    public override void _Ready()
    {
        Velocity = new Godot.Vector2(150, -300); // set initial velocity to up and slightly right
    }

	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			var collider = collision.GetCollider(); //colliding obj

			if (collider is Paddle paddle)
			{
				var paddleWidth = 100f;

				var offset = (Position.X - paddle.Position.X) / (paddleWidth / 2f);
				offset = Mathf.Clamp(offset, -1f, 1f);

				var bounceAngle = offset * Mathf.Pi / 3f; //max 60deg bounce
				Velocity = new Godot.Vector2(
					Speed * Mathf.Sin(bounceAngle),
					-Speed * Mathf.Cos(bounceAngle)
				);
			}
			else
			{
				Velocity = Velocity.Bounce(collision.GetNormal());
			}
		}
	}
}
