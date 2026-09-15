using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Signal]
	public delegate void BrickDestroyedEventHandler(int points);
	
	[Export] public int PointValue { get; private set; } = 10;
	public Vector2 Size
	{
		get
		{
			var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
			return collisionShape.Shape is RectangleShape2D rect 
				? rect.Size * this.Scale 
				: Vector2.Zero;
		}
	}
	
	public void Hit()
	{
		EmitSignal(SignalName.BrickDestroyed, PointValue);
		QueueFree();
	}
}
