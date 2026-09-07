using Godot;
using System;

public partial class Main : Node2D
{
	[Export] private PackedScene _ballScene;
	private int _lives = 3;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnNewBall();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBallLost()
	{
		GD.Print("OnBallLost Called!");
		_lives--;
		GD.Print("lives: ", _lives);

		if (_lives <= 0)
		{
			GameOver();
		}
		else 
		{
			SpawnNewBall();
		}
		
	}

	private void SpawnNewBall()
	{
		var ball = _ballScene.Instantiate<Ball>();
		ball.Position = new Vector2(400, 400);
		AddChild(ball);
	}

	private void GameOver()
	{
		GD.Print("Game Over");
	}
}
