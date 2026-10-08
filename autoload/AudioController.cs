using System;
using Godot;

namespace brickpong2D.autoload;

public partial class AudioController : Node
{
	[Export] private resources.SoundLibrary _soundLibrary;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ValidateExports();

		EventBus.Instance.BallHitPaddle += OnBallHitPaddle;
		EventBus.Instance.BallHitWall += OnBallHitWall;
		EventBus.Instance.BrickBroken += OnBrickBroken;
		EventBus.Instance.LifeLost += OnLifeLost;
		EventBus.Instance.Victory += OnVictory;
		EventBus.Instance.ButtonHovered += OnButtonHovered;
		EventBus.Instance.ButtonPressed += OnButtonPressed;

		GameManager.Instance.GameOver += OnGameOver;
	}

	private void ValidateExports()
	{
		if (_soundLibrary == null)
			throw new Exception("SoundLibrary not set in AudioController.");
		
		_soundLibrary.Validate();
	}

	private void OnBallHitPaddle(Vector2 position) =>
		SoundManager.Instance.PlaySfx(_soundLibrary.PaddleBounce, position);
	private void OnBallHitWall(Vector2 position) => 
		SoundManager.Instance.PlaySfx(_soundLibrary.WallBounce, position);
	private void OnBrickBroken(Vector2 position) => 
		SoundManager.Instance.PlaySfx(_soundLibrary.BrickBreak, position);
	private void OnLifeLost() => 
		SoundManager.Instance.PlaySfx(_soundLibrary.LifeLost);
	private void OnVictory() => 
		SoundManager.Instance.PlaySfx(_soundLibrary.Victory);
	private void OnGameOver() => 
		SoundManager.Instance.PlaySfx(_soundLibrary.GameOver);
	
	// UI
	private void OnButtonHovered() => 
		SoundManager.Instance.PlayUISfx(_soundLibrary.ButtonHover);
	private void OnButtonPressed() => 
		SoundManager.Instance.PlayUISfx(_soundLibrary.ButtonClick);

	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.BallHitPaddle -= OnBallHitPaddle;
			EventBus.Instance.BallHitWall -= OnBallHitWall;
			EventBus.Instance.BrickBroken -= OnBrickBroken;
			EventBus.Instance.LifeLost -= OnLifeLost;
			EventBus.Instance.ButtonHovered -= OnButtonHovered;
			EventBus.Instance.ButtonPressed -= OnButtonPressed;
			EventBus.Instance.Victory -= OnVictory;
		}
		if (GameManager.Instance != null)
			GameManager.Instance.GameOver -= OnGameOver;
	}
}