using Godot;
using System;

public partial class Main : Node2D
{
	[Export] private PackedScene _ballScene;
	[Export] private Hud _hud;
	[Export] private BrickGrid _brickGrid;
	private int _lives = 3;
	private int _score = 0;

	private NodePool<Ball> _ballPool;

	public override void _Ready()
	{
		_ballPool = new NodePool<Ball>(_ballScene, this);

		//Check scenes have been set
		GD.Print($"\nin Main -- _ballScene: {_ballScene}\n _hud: {_hud}\n _brickGrid: {_brickGrid}");
		
		//Show grid in first launch
		_brickGrid.ResetGrid();
		_brickGrid.BrickDestroyed += OnBrickDestroyed;
		_hud.ShowStartGame();
	}
	
	private void NewGame()
	{
		if (_ballScene == null || !_ballScene.CanInstantiate())
		{
			GD.PushError("BallScene has not been instantiated.");
			return;
		}
		
		_brickGrid.ResetGrid();
		_lives = 3;
		_score = 0;
		_hud.UpdateScore(_score);
		_hud.UpdateLives(_lives);
		
		SpawnNewBall();
	}
	

	private void OnBallLost(Node2D lostBall)
	{
		_ballPool.Release((Ball)lostBall);

		_lives--;
		_hud.UpdateLives(_lives);

		if (_lives <= 0)
		{
			_hud.ShowGameOver();
		}
		else 
		{
			SpawnNewBall();
		}
		
	}
	
	private void SpawnNewBall()
	{
		var ball = _ballPool.Acquire();
		ball.Position = new Vector2(400, 400);
	}
	
	private void OnBrickDestroyed(int points)
	{
		_score += points;
		_hud.UpdateScore(_score);
	}
}
