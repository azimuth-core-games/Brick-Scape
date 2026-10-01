using System;
using brickpong2D.autoload;
using brickpong2D.scenes.ui;
using Godot;

namespace brickpong2D.scenes.main_menu;

public partial class MainMenu : Node
{
    [Export] private UiButton  _startButton;
    [Export] private UiButton _quitButton;
    [Export] private AudioStream _audioStream;

    public override void _Ready()
    {
        ValidateExports();
        
        _startButton.Pressed += OnStartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
        
        SoundManager.Instance.PlayMusic(_audioStream);
    }

    private void ValidateExports()
    {
        if (_startButton == null)
            throw new SystemException("_startButton is not set in MainMenu.");
        if (_quitButton == null)
            throw new SystemException("_quitButton is not set in MainMenu.");
        if (_audioStream == null)
            throw new SystemException("_audioStream is not set in MainMenu.");
    }

    private void OnStartButtonPressed() =>
        SceneManager.Instance.GoToLevel1();

    private void OnQuitButtonPressed() => 
        GetTree().Quit();
}