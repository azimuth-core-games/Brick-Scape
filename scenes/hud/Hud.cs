using Godot;
using System;
using System.Threading.Tasks;
using brickpong2D.scenes.hud.lives;

public partial class Hud : CanvasLayer
{
	[Export] private Label _scoreLabel;
	[Export] private Label _messageLabel;
	[Export] private LivesDisplay _livesDisplay;

	public override void _Ready()
	{
		//Check nodes have been set
		ValidateExports();
	}

	private void ValidateExports()
	{
		if (_scoreLabel == null)
			throw new SystemException("_scoreLabel is not set in Hud.");
		if (_messageLabel == null)
			throw new SystemException("_messageLabel is not set in Hud.");
		if (_livesDisplay == null)
			throw new SystemException("_livesDisplay is not set in Hud.");
	}

	public void UpdateScore(int score)
	{
		var scoreString = "Score: " + score.ToString();
		_scoreLabel.Text = scoreString;
	}

	public void UpdateLives(int lives)
	{
		_livesDisplay.ShowLives(lives);
	}

	/// <summary>
	/// Returns awaitable task to ensure completion before continuing.
	/// </summary>
	/// <param name="message">string message to display</param>
	/// <returns></returns>
	public async Task ShowTimedMessage(string message)
	{
		_messageLabel.Text = message;
		_messageLabel.Show();

		await ToSignal(GetTree().CreateTimer(2f), Timer.SignalName.Timeout);

		// check if message has been freed or HUD removed from scene tree.
		// In this case return (nothing can be done to _messageLabel)
		if (!IsInstanceValid(_messageLabel) || !IsInsideTree())
			return;

		_messageLabel.Hide();
	}

	public async Task ShowGameOver()
	{
		try
		{
			await ShowTimedMessage("Game Over");
		}
		catch (Exception e)
		{
			GD.PushError($"ShowGameOver Error: {e}");
		}
	}
}
