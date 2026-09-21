using Godot;

namespace brickpong2D.autoload;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3;

    [Signal]
    public delegate void ScoreChangedEventHandler(int newScore);
    [Signal]
    public delegate void LivesChangedEventHandler(int newLives);
    [Signal]
    public delegate void GameOverEventHandler();

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            GD.PushWarning("Duplicate Game Manager instance! - Freeing new one.");
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void ResetGame()
    {
        this.Score = 0;
        this.Lives = 3;
        EmitSignal(SignalName.ScoreChanged, this.Score);
        EmitSignal(SignalName.LivesChanged, this.Lives);
    }

    public void AddScore(int points)
    {
        this.Score += points;
        EmitSignal(SignalName.ScoreChanged, this.Score);
    }

    public void LoseLife()
    {
        this.Lives--;
        EmitSignal(SignalName.LivesChanged, this.Lives);
        
        if (this.Lives <= 0)
            EmitSignal(SignalName.GameOver);
    }

    public override void _ExitTree()
    {
        if (Instance == this)
            Instance = null;
    }
}