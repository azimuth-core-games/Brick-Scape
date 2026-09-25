using System;
using Godot;

namespace brickpong2D.scenes.main_menu;

public partial class MainMenu : Node
{
    [Export] private Button _startButton;
    [Export] private Button _quitButton;

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

    private void OnStartButtonPressed()
    {
        GD.Print("Start button pressed");
        if (brickpong2D.autoload.SceneManager.Instance == null)
        {
            GD.PrintErr("Scene Manager is not set.");
            return;
        }
        brickpong2D.autoload.SceneManager.Instance.GoToLevel1();
    }

    private void OnQuitButtonPressed()
    {
        GD.Print("Quit button pressed");
        GetTree().Quit();
    }
}