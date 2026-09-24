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
	private GameManager _gameManager;
	
	private NodePool<ball.Ball> _ballPool;

	public override void _Ready()
	{
		//Check scenes have been set in editor
		ValidateExports();
		
		_ballPool = new NodePool<ball.Ball>(_ballScene, this);
		_gameManager = GameManager.Instance;

		if (_gameManager == null)
		{
			throw new SystemException("GameManager is null in MainLevel. Check that it is registered in autoloads.");
		}

		_gameManager.ScoreChanged += OnScoreChanged;
		_gameManager.LivesChanged += OnLivesChanged;
		_gameManager.GameOver += OnGameOver;
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
	}
	
	private void NewGame()
	{
		if (_ballScene == null || !_ballScene.CanInstantiate())
		{
			GD.PushError("BallScene has not been instantiated.");
			return;
		}
		
		_brickGrid.ResetGrid();
		_gameManager.ResetGame();
		
		SpawnNewBall();
	}
	

	private void OnBallLost(Node2D ball)
	{
		_ballPool.Release((brickpong2D.scenes.ball.Ball)ball);
		GD.Print($"Ball pool decreased to {_ballPool.TotalCreated}, available: {_ballPool.AvailableCount}");
		
		_gameManager.LoseLife();

		if (_gameManager.Lives > 0)
			SpawnNewBall();
	}
	
	private void SpawnNewBall()
	{
		var ball = _ballPool.Acquire();
		ball.Position = new Vector2(400, 400);
		
		GD.Print($"Ball pool increased to {_ballPool.TotalCreated}, available: {_ballPool.AvailableCount}");
	}
	
	private void OnBrickDestroyed(int points)
	{
		_gameManager.AddScore(points);
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
		if (_gameManager == null)
			return;
		
		_gameManager.ScoreChanged -= OnScoreChanged;
		_gameManager.LivesChanged -= OnLivesChanged;
		_gameManager.GameOver -= OnGameOver;
	}
}