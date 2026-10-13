using Godot;

namespace brickpong2D.autoload;

public partial class EventBus : Node
{
    public static EventBus Instance { get; private set; }
    
    // Game events
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal]
    public delegate void BallHitPaddleEventHandler(Vector2 position);
    [Signal]
    public delegate void BallHitWallEventHandler(Vector2 position);
    [Signal]
    public delegate void BrickBrokenEventHandler(Vector2 position);
    [Signal]
    public delegate void LifeLostEventHandler();
    [Signal]
    public delegate void VictoryEventHandler();
    
    // UI events
    [Signal]
    public delegate void ButtonHoveredEventHandler();
    [Signal]
    public delegate void ButtonPressedEventHandler();
    [Signal]
    public delegate void MainMenuScreenEventHandler();
    [Signal]
    public delegate void QuitGameEventHandler();
    
    

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
            Instance = null;
    }
}