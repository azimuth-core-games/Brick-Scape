using System;
using Godot;

namespace brickpong2D.autoload;

public partial class SceneManager : Node
{
    public static SceneManager Instance { get; private set; }
    
    [Export]
    private PackedScene _mainMenuScene;

    [Export] private PackedScene _level1Scene;
    [Export] private PackedScene _victoryScene;

    public override void _Ready()
    {
        Instance = this;
        ValidateExports();
        
        EventBus.Instance.Victory += GoToVictory;
    }

    private void ValidateExports()
    {
        if (_mainMenuScene == null)
            throw new SystemException("_mainMenuScene is not set in SceneManager.");
        if (_level1Scene == null)
            throw new SystemException("_level1Scene is not set in SceneManager.");
        if (_victoryScene == null)
            throw new SystemException("_victoryScene is not set in SceneManager.");
    }

    public void GoToMainMenu()
    {
        GetTree().ChangeSceneToPacked(_mainMenuScene);
    }

    public void GoToLevel1()
    {
        GetTree().ChangeSceneToPacked(_level1Scene);
    }

    private void GoToVictory()
    {
        GetTree().ChangeSceneToPacked(_victoryScene);
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Victory -= GoToVictory;
        }
    }

}
