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
	private GameManager _gameManager;
	
	private NodePool<ball.Ball> _ballPool;

	public override void _Ready()
	{
		//Check scenes have been set in editor
		ValidateExports();
		
		SoundManager.Instance.StopMusic();
		
		_ballPool = new NodePool<ball.Ball>(_ballScene, this);
		_gameManager = GameManager.Instance;

		_killZone.BallLost += OnBallLost;
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
		_gameManager.ResetGame();
		
		SpawnNewBall();
	}
	

	private void OnBallLost(Node2D ball)
	{
		SoundManager.Instance.PlaySfx(Sfx.LifeLost);
		_ballPool.Release((ball.Ball)ball);
		
		_gameManager.LoseLife();
		if (_gameManager.Lives > 0)
			SpawnNewBall();
	}
	
	private void SpawnNewBall()
	{
		var ball = _ballPool.Acquire();
		ball.Position = new Vector2(400, 400);
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
			SoundManager.Instance.PlaySfx(Sfx.GameOver);
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
