using Godot;
using System;
using brickpong2D.autoload;

public partial class Victory : Node2D
{
	[Export] private Button _mainMenuButton;

	public override void _Ready()
	{
		ValidateExports();

		_mainMenuButton.Pressed += OnMainMenuButtonPressed;
	}

	private void ValidateExports()
	{
		if (_mainMenuButton == null)
			throw new SystemException("MainMenuButton is not set in Victory scene.");
	}

	private void OnMainMenuButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.MainMenuScreen);
	}
}
