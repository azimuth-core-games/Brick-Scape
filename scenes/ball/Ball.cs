using brickpong2D.scenes.common;
using Godot;

namespace brickpong2D.scenes.ball;

public partial class Ball : CharacterBody2D, IPoolable
{
	[Export] public float Speed { get; set; } = 400.0f;

	[Export] public Vector2 InitialVelocity { get; set; } = new Vector2(150, -300);

	//Set difficulty (higher angle => harder)
	[Export] private float _maxBounceAngleDegrees = 60f;
	private float MaxBounceAngle => Mathf.DegToRad(_maxBounceAngleDegrees);
	private CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public void OnAcquire()
	{
		Visible = true;
		SetPhysicsProcess(true);
		Velocity = InitialVelocity;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}

	public void OnRelease()
	{
		Visible = false;
		SetPhysicsProcess(false);
		Velocity = Vector2.Zero;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision is null) return;

		var collider = collision.GetCollider(); //colliding obj
		switch (collider)
		{
			case Paddle paddle:
				HandlePaddleCollision(paddle, collision);
				break;
			case brickpong2D.scenes.brick.Brick brick:
				brick.Hit();
				Velocity = Velocity.Bounce(collision.GetNormal());
				break;
			default:
				Velocity = Velocity.Bounce(collision.GetNormal());
				break;
		}
	}

	private void HandlePaddleCollision(Paddle paddle, KinematicCollision2D collision)
	{
		if (paddle.Width <= 0f)
		{
			GD.PushWarning("Paddle width 0. Using default bounce function.");
			Velocity = Velocity.Bounce(collision.GetNormal());
			return;
		}
		
		// Calculate where on paddle ball hit as a number from [-1,1]
		var normalizedHitPosition = (Position.X - paddle.Position.X) / (paddle.Width / 2f);
		normalizedHitPosition = Mathf.Clamp(normalizedHitPosition, -1f, 1f);

		// Get a different angle depending on where ball hit paddle
		var bounceAngle = normalizedHitPosition * MaxBounceAngle;
		
		// Using trig to find new x and y. Sin(angle) = x/Speed and Cos(angle) = -y/Speed
		Velocity = new Vector2(Speed * Mathf.Sin(bounceAngle), -Speed * Mathf.Cos(bounceAngle));
	}

}