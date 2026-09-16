using Godot;
using System;
using System.Threading.Tasks;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	[Export] private Button _startButton;
	[Export] private Label _scoreLabel;
	[Export] private Label _livesLabel;
	[Export] private Label _messageLabel;

	public override void _Ready()
	{
		//Check nodes have been set
		GD.Print($"\nin Hud -- _startButton: {_startButton}\n _scoreLabel: {_scoreLabel}\n _livesLabel: {_livesLabel}\n _messageLabel: {_messageLabel}");
	}

	public void UpdateScore(int score)
	{
		var scoreString = "Score: " + score.ToString();
		_scoreLabel.Text = scoreString;
	}

	public void UpdateLives(int lives)
	{
		var livesString = "Lives: " + lives.ToString();
		_livesLabel.Text = livesString;
	}

	private void OnStartButtonPressed()
	{
		_startButton.Hide();
		EmitSignal(SignalName.StartGame);
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

	public async void ShowGameOver()
	{
		try
		{
			await ShowTimedMessage("Game Over");
			ShowStartGame();
		}
		catch (Exception e)
		{
			GD.PushError($"ShowGameOver Error: {e}");
		}
	}
	
	public void ShowStartGame()
	{
		_startButton.Show();
	}
}
