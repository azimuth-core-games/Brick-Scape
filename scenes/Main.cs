using Godot;
using System;

public partial class Main : Node2D
{
	[Export] private PackedScene _ballScene;
	[Export] private Hud _hud;
	[Export] private BrickGrid _brickGrid;
	private int _lives = 3;
	private int _score = 0;

	public override void _Ready()
	{
		//Check scenes have been set in editor
		ValidateExports();
		
		//Show grid in first launch
		_brickGrid.ResetGrid();
		_brickGrid.BrickDestroyed += OnBrickDestroyed;
		_hud.ShowStartGame();
	}

	private void ValidateExports()
	{
		if (_ballScene == null)
			throw new SystemException("_ballScene is not set in Main.");
		if (_brickGrid == null)
			throw new SystemException("_brickGrid is not set in Main.");
		if (_hud == null)
			throw new SystemException("_hud is not set in Main.");
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
	

	private void OnBallLost()
	{
		_lives--; 
		_hud.UpdateLives(_lives);

		if (_lives <= 0)
		{
			_hud.ShowGameOver();
		}
		else 
		{
			// Spawning new ball touches when ball is still in Killzone
			// Defer until processes are done and there's idle time.
			CallDeferred(nameof(SpawnNewBall));
		}
		
	}
	
	private void SpawnNewBall()
	{
		var ball = _ballScene.Instantiate<Ball>();
		ball.Position = new Vector2(400, 400);
		AddChild(ball);
	}
	
	private void OnBrickDestroyed(int points)
	{
		_score += points;
		_hud.UpdateScore(_score);
	}
}
