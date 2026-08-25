using Godot;
using System;
using System.Numerics;

public partial class Paddle : CharacterBody2D
{
	[Export]
	public float Speed = 400.0f;

	public override void _PhysicsProcess(double delta)
	{
		float direction = Input.GetAxis("move_left", "move_right");
		Velocity = new Godot.Vector2(direction * Speed, 0);
		MoveAndSlide();

		var viewportWidth = GetViewportRect().Size.X;
		var halfWidth = GetNode<CollisionShape2D>("CollisionShape2D").Shape.GetRect().Size.X / 2;

		Position = new Godot.Vector2(
			Mathf.Clamp(Position.X, halfWidth, viewportWidth - halfWidth),
			Position.Y
		);
	}
}
