using Godot;
using System;
using brickpong2D.scenes.common;

public partial class BrickGrid : Node2D
{
	// Pass signal up from each individual brick
	[Signal]
	public delegate void BrickDestroyedEventHandler(int points);
	
	// allows selection of scene to use for grid (Brick)
	[Export] private PackedScene _brickScene;

	private int _rows = 8;
	private int _cols = 16;
	private int _brickSpacing = 4;
	private Vector2 _brickSize;

	// Padding added to top of grid in viewport
	private int _paddingTopPixels = 50;
	
	private NodePool<Brick> _brickPool;

	public override void _Ready()
	{
		//Check PackedScene set in editor
		ValidateExports();
		_brickPool = new NodePool<Brick>(_brickScene, this,
			onCreate: brick => brick.BrickDestroyed += OnBrickDestroyed);
		GetBrickSize();
	}

	private void ValidateExports()
	{
		if (_brickScene == null) throw new SystemException($"_brickScene is not set in BrickGrid.");
	}

	private void GetBrickSize()
	{
		// Get size of a temp brick and then delete.
		var tempBrick = _brickScene.Instantiate<Brick>();
		AddChild(tempBrick); // needed to get the Brick's Size from _Ready()
		_brickSize = tempBrick.Size;
		tempBrick.QueueFree();
	}

	public void ResetGrid()
	{
		_brickPool.DespawnAll();
		GenerateBrickGrid();
	}

	private void CenterGrid()
	{
		var totalGridSize = new Vector2(_cols * _brickSize.X, _rows * _brickSize.Y);
		var viewPortSize = GetViewportRect().Size;
		
		Position = new Vector2((viewPortSize.X - totalGridSize.X)/ 2f, _paddingTopPixels);
	}

	private void GenerateBrickGrid()
	{
		for (var row = 0; row < _rows; row++)
		{
			for (var col = 0; col < _cols; col++)
			{
				var brick = _brickPool.Spawn();

				if (_brickSize == Vector2.Zero)
					_brickSize = brick.Size;
				
				brick.Position = new Vector2(
					col * (_brickSize.X + _brickSpacing), 
					row * (_brickSize.Y + _brickSpacing)
					);
			}
		}
		
		CenterGrid();
	}

	private void OnBrickDestroyed(Brick brick, int points)
	{
		GD.Print($"Bricks created: {_brickPool.TotalCreated}, available: {_brickPool.AvailableCount}");
		_brickPool.Despawn(brick);
		EmitSignal(SignalName.BrickDestroyed, points);
	}
}
