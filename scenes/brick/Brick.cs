using Godot;
using System;
using brickpong2D.scenes.common;

public partial class Brick : StaticBody2D, IPoolable
{
	[Signal]
	public delegate void BrickDestroyedEventHandler(Brick brick, int points);
	
	[Export] public int PointValue { get; private set; } = 10;
	private CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public void OnSpawn()
	{
		Visible = true;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}

	public void OnDespawn()
	{
		Visible = false;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	
	public Vector2 Size =>
		_collisionShape.Shape is RectangleShape2D rect 
			? rect.Size * this.Scale
			: Vector2.Zero;


	public void Hit()
	{
		EmitSignal(SignalName.BrickDestroyed, this, PointValue);
	}
}
