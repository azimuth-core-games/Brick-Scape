using Godot;
using System;

public partial class Killzone : Area2D
{
	[Signal]
	public delegate void BallLostEventHandler();

	private void OnBodyEntered(Node2D body)
	{
		
		if (!body.IsInGroup("ball")) return;
		
		body.QueueFree();
		EmitSignal(SignalName.BallLost);
	}
}
