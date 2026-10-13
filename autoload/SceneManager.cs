using System;
using Godot;

namespace brickpong2D.autoload;

public partial class SceneManager : Node
{
    [Export] private PackedScene _mainMenuScene;
    [Export] private PackedScene _level1Scene;
    [Export] private PackedScene _victoryScene;

    public override void _Ready()
    {
        ValidateExports();
        
        EventBus.Instance.Victory += GoToVictory;
        EventBus.Instance.StartGame += GoToLevel1;
        EventBus.Instance.MainMenuScreen += GoToMainMenu;
        EventBus.Instance.QuitGame += QuitGame;
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

    private void GoToMainMenu()
    {
        GetTree().ChangeSceneToPacked(_mainMenuScene);
    }

   private void GoToLevel1()
    {
        GetTree().ChangeSceneToPacked(_level1Scene);
    }

    private void GoToVictory()
    {
        GetTree().ChangeSceneToPacked(_victoryScene);
    }
    
    private void QuitGame() => GetTree().Quit();

    public override void _ExitTree()
    {
        EventBus.Instance.Victory -= GoToVictory;
        EventBus.Instance.MainMenuScreen -= GoToMainMenu;
        EventBus.Instance.StartGame -= GoToLevel1;
        EventBus.Instance.QuitGame -= QuitGame;
    }

}
