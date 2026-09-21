using Godot;

namespace brickpong2D.scenes;

public partial class Killzone : Area2D
{
	[Signal]
	public delegate void BallLostEventHandler(Node2D ball);

	private void OnBodyEntered(Node2D body)
	{
		
		if (!body.IsInGroup("ball")) return;
		
		EmitSignal(SignalName.BallLost, body);
	}
}