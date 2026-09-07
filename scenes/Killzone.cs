using Godot;
using System;

public partial class Killzone : Area2D
{
	[Signal]
	public delegate void BallLostEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Body entered KILLZONE: ", body.Name);
		
		if (!body.IsInGroup("ball")) return;
		
		GD.Print("Ball detected, emitting BallLost");
		body.QueueFree();
		EmitSignal(SignalName.BallLost);
	}
}
