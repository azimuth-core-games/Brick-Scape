using System;
using brickpong2D.autoload;
using brickpong2D.common;
using Godot;

namespace brickpong2D.scenes;

public partial class MainLevel : Node2D
{
	[Export] private PackedScene _ballScene;
	[Export] private Hud _hud;
	[Export] private BrickGrid _brickGrid;
	[Export] private Killzone _killZone;
	
	private NodePool<ball.Ball> _ballPool;

	public override void _Ready()
	{
		//Check scenes have been set in editor
		ValidateExports();
		
		SoundManager.Instance.StopMusic();
		
		_ballPool = new NodePool<ball.Ball>(_ballScene, this);

		_killZone.BallLost += OnBallLost;
		GameManager.Instance.ScoreChanged += OnScoreChanged;
		GameManager.Instance.LivesChanged += OnLivesChanged;
		GameManager.Instance.GameOver += OnGameOver;
		_brickGrid.BrickDestroyed += OnBrickDestroyed;
		
		NewGame();
	}

	private void ValidateExports()
	{
		if (_ballScene == null)
			throw new SystemException("_ballScene is not set in Main.");
		if (_brickGrid == null)
			throw new SystemException("_brickGrid is not set in Main.");
		if (_hud == null)
			throw new SystemException("_hud is not set in Main.");
		if (_killZone == null)
			throw new SystemException("_killZone is not set in Main.");
	}
	
	private void NewGame()
	{
		if (!_ballScene.CanInstantiate())
		{
			throw new SystemException("BallScene can not be instantiated.");
		}
		
		_brickGrid.ResetGrid();
		GameManager.Instance.ResetGame();
		
		SpawnNewBall();
	}
	

	private void OnBallLost(Node2D ball)
	{
		_ballPool.Release((ball.Ball)ball);
		
		GameManager.Instance.LoseLife();
		if (GameManager.Instance.Lives > 0)
		{
			EventBus.Instance.EmitSignal(EventBus.SignalName.LifeLost);
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
		GameManager.Instance.AddScore(points);
	}
	
	private void OnScoreChanged(int newScore) => _hud.UpdateScore(newScore);
	private void OnLivesChanged(int newLives) => _hud.UpdateLives(newLives);

	private async void OnGameOver()
	{
		try
		{
			await _hud.ShowGameOver();
		}
		catch (Exception e)
		{
			GD.PrintErr("Error printing GameOver in MainLevel:", e.ToString());
		}
		
		SceneManager.Instance.GoToMainMenu();
	}

	// Unsubscribe from signals when game finishes.
	// Prevents sending signals to previous scene instances.
	public override void _ExitTree()
	{
		if (GameManager.Instance == null)
			return;
		
		GameManager.Instance.ScoreChanged -= OnScoreChanged;
		GameManager.Instance.LivesChanged -= OnLivesChanged;
		GameManager.Instance.GameOver -= OnGameOver;
	}
}
