
using brickpong2D.autoload;
using brickpong2D.common;
using Godot;

namespace brickpong2D.scenes.brick;

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

	public void OnAcquire()
	{
		Visible = true;
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}

	public void OnRelease()
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
		EventBus.Instance.EmitSignal(EventBus.SignalName.BrickBroken, GlobalPosition);
		EmitSignal(SignalName.BrickDestroyed, this, PointValue);
	}
}