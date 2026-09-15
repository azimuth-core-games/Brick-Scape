using Godot;
using System;

public partial class BrickGrid : Node2D
{
	// Pass signal up from each individual brick
	[Signal]
	public delegate void BrickDestroyedEventHandler(int points);
	
	// allows selection of scene to use for grid (Brick)
	[Export] private PackedScene _brickScene;

	private int _rows = 8;
	private int _cols = 16;
	private Vector2 _brickSize;

	// Padding added to top of grid in viewport
	private int _paddingTopPixels = 50;

	public override void _Ready()
	{
		
		//Check PackedScene set in editor
		GD.Print($"in BrickGrid -- BrickGridScene: {_brickScene}");
	
	}

	public void ResetGrid()
	{
		ClearBricks();
		GenerateBrickGrid();
	}

	private void CenterGrid()
	{
		var totalGridSize = new Vector2(_cols * _brickSize.X, _rows * _brickSize.Y);
		var viewPortSize = GetViewportRect().Size;
		
		Position = new Vector2((viewPortSize.X - totalGridSize.X)/ 2f, _paddingTopPixels);
	}

	private void ClearBricks()
	{
		foreach (Node child in GetChildren())
		{
			if (child is Brick brick)
			{
				brick.QueueFree();
			}
		}
	}

	private void GenerateBrickGrid()
	{
		var brickSpacing = 4f;
		
		// Get brick size once
		var sampleBrick = _brickScene.Instantiate<Brick>();
		_brickSize = sampleBrick.Size;
		
		for (var row = 0; row < _rows; row++)
		{
			for (var col = 0; col < _cols; col++)
			{
				var brick = _brickScene.Instantiate<Brick>();
				
				brick.Position = new Vector2(
					col * (_brickSize.X + brickSpacing), 
					row * (_brickSize.Y + brickSpacing)
					);
				brick.BrickDestroyed += OnBrickDestroyed;
				AddChild(brick);
			}
		}
		
		CenterGrid();
	}

	private void OnBrickDestroyed(int points)
	{
		EmitSignal(SignalName.BrickDestroyed, points);
	}
}
