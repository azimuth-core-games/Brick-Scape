using System;
using brickpong2D.autoload;
using brickpong2D.scenes.ui;
using Godot;

namespace brickpong2D.scenes.main_menu;

public partial class MainMenu : Node
{
    [Export] private UiButton  _startButton;
    [Export] private UiButton _quitButton;

    public override void _Ready()
    {
        ValidateExports();
        
        _startButton.Pressed += OnStartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private void ValidateExports()
    {
        if (_startButton == null)
            throw new SystemException("_startButton is not set in MainMenu.");
        if (_quitButton == null)
            throw new SystemException("_quitButton is not set in MainMenu.");
    }

    private void OnStartButtonPressed() =>
        EventBus.Instance.EmitSignal(EventBus.SignalName.StartGame);

    private void OnQuitButtonPressed() => 
        EventBus.Instance.EmitSignal(EventBus.SignalName.QuitGame);
}