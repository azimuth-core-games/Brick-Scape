using System;
using Godot;

namespace brickpong2D.autoload;

public partial class SceneManager : Node
{
    public static SceneManager Instance { get; private set; }
    
    [Export]
    private PackedScene _mainMenuScene;

    [Export] private PackedScene _level1Scene;

    public override void _Ready()
    {
        Instance = this;
    }

    private void ValidateExports()
    {
        if (_mainMenuScene == null)
            throw new SystemException("_mainMenuScene is not set in SceneManager.");
        if (_level1Scene == null)
            throw new SystemException("_level1Scene is not set in SceneManager.");
    }

    public void GoToMainMenu()
    {
        if (_mainMenuScene == null)
        {
            GD.PrintErr("Main menu scene is not set in SceneManager.");
            return;
        }
        GetTree().ChangeSceneToPacked(_mainMenuScene);
    }

    public void GoToLevel1()
    {
        if (_level1Scene == null)
        {
            GD.PrintErr("Level 1 scene is not set in SceneManager.");
            return;
        }
        GetTree().ChangeSceneToPacked(_level1Scene);
    }

}