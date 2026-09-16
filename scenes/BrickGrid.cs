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

	private NodePool<Brick> _brickPool;

	public override void _Ready()
	{
		_brickPool = new NodePool<Brick>(_brickScene, this,
			onCreate: brick => brick.BrickDestroyed += points => OnBrickDestroyed(brick, points));

		//Check PackedScene set in editor
		GD.Print($"in BrickGrid -- BrickGridScene: {_brickScene}");

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
		var brickSpacing = 4f;

		for (var row = 0; row < _rows; row++)
		{
			for (var col = 0; col < _cols; col++)
			{
				var brick = _brickPool.Acquire();

				if (_brickSize == Vector2.Zero)
				{
					_brickSize = brick.Size;
				}

				brick.Position = new Vector2(
					col * (_brickSize.X + brickSpacing),
					row * (_brickSize.Y + brickSpacing)
					);
			}
		}

		CenterGrid();
	}

	private void OnBrickDestroyed(Brick brick, int points)
	{
		_brickPool.Release(brick);
		EmitSignal(SignalName.BrickDestroyed, points);
	}
}
