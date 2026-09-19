using System;
using brickpong2D.autoload;
using Godot;

namespace brickpong2D.scenes.main;

public partial class MainMenu : Control
{
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal]
    public delegate void ExitGameEventHandler();
    
    [Export] private Button _startButton;
    [Export] private Button _quitButton;
    [Export] private AudioStream _themeMusic;

    public override void _Ready()
    {
        ValidateExports();
        
        SoundManager.Instance.PlayMusic(_themeMusic);
        
        _startButton.Pressed += OnStartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private void ValidateExports()
    {
        if (_startButton == null)
            throw new SystemException("_startButton is not set in Main Menu.");
        if (_quitButton == null)
            throw new SystemException("_quitButton is not set in Main Menu.");
        if (_themeMusic == null)
            throw new SystemException("ThemeMusic is not set in Main Menu.");
    }

    private void OnStartButtonPressed()
    {
        // Change scene to main level
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }

    private void OnQuitButtonPressed()
    {   
        GetTree().Quit();
    }
}