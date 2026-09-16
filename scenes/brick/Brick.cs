using Godot;
using System;

public partial class Brick : StaticBody2D, IPoolable
{
	[Signal]
	public delegate void BrickDestroyedEventHandler(int points);

	[Export] public int PointValue { get; private set; } = 10;

	private CollisionShape2D _collisionShape;

	public Vector2 Size => _collisionShape.Shape is RectangleShape2D rect
		? rect.Size * Scale
		: Vector2.Zero;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public void Hit()
	{
		EmitSignal(SignalName.BrickDestroyed, PointValue);
	}

	public void OnAcquire()
	{
		Visible = true;
		_collisionShape.Disabled = false;
	}

	public void OnRelease()
	{
		Visible = false;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
