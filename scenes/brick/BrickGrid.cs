using System;
using brickpong2D.autoload;
using brickpong2D.common;
using brickpong2D.scenes.brick;
using Godot;

namespace brickpong2D.scenes;

public partial class BrickGrid : Node2D
{
	// Pass signal up from each individual brick
	[Signal]
	public delegate void BrickDestroyedEventHandler(int points);
	
	// allows selection of scene to use for grid (Brick)
	[Export] private PackedScene _brickScene;
	[Export] private int _rows = 7;
	[Export] private int _cols = 16;
	[Export] private int _brickSpacing = 4;

	// Padding added to top of grid in viewport
	[Export] private int _paddingTopPixels = 75;
	
	private NodePool<brick.Brick> _brickPool;
	private Vector2 _brickSize;

	public override void _Ready()
	{
		//Check PackedScene set in editor
		ValidateExports();
		_brickPool = new NodePool<brick.Brick>(_brickScene, this,
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
		var tempBrick = _brickScene.Instantiate<brick.Brick>();
		AddChild(tempBrick); // needed to get the Brick's Size from _Ready()
		_brickSize = tempBrick.Size;
		tempBrick.QueueFree();
	}

	public void ResetGrid()
	{
		_brickPool.ReleaseAll();
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
				var brick = _brickPool.Acquire();
		
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
		_brickPool.Release(brick);
		EmitSignal(SignalName.BrickDestroyed, points);

		if (_brickPool.AvailableCount == _brickPool.TotalCreated)
		{
			// All bricks have been broken, signal Victory
			EventBus.Instance.EmitSignal(EventBus.SignalName.Victory);
		}
	}
}