using System;
using brickpong2D.autoload;
using Godot;

namespace brickpong2D.scenes.main_menu;

public partial class MainMenu : Node
{
    [Export] private Button _startButton;
    [Export] private Button _quitButton;
    // [Export] private AudioStream _audioStream;

    public override void _Ready()
    {
        ValidateExports();
        
        _startButton.Pressed += OnStartButtonPressed;
        _startButton.FocusEntered += OnButtonHovered;
        _quitButton.Pressed += OnQuitButtonPressed;
        _quitButton.FocusEntered += OnButtonHovered;
        
        // SoundManager.Instance.PlayMusic(_audioStream);
    }

    private void ValidateExports()
    {
        if (_startButton == null)
            throw new SystemException("_startButton is not set in MainMenu.");
        if (_quitButton == null)
            throw new SystemException("_quitButton is not set in MainMenu.");
    }

    private void OnButtonHovered()
    {
        GD.Print("OnButtonHovered");
        SoundManager.Instance.PlayUISfx(Sfx.ButtonHover);
    }

    private void OnStartButtonPressed()
    {
        GD.Print("OnStartButtonPressed");
        SoundManager.Instance.PlayUISfx(Sfx.ButtonClick);
        SceneManager.Instance.GoToLevel1();
    }

    private void OnQuitButtonPressed()
    { 
        GD.Print("OnQuitButtonPressed");
        SoundManager.Instance.PlayUISfx(Sfx.ButtonClick);
        GetTree().Quit();
    }
}