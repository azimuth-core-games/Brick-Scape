using Godot;
using System;

public partial class BrickGrid : Node2D
{
	// allows selection of scene to use for grid (Brick)
	[Export] public PackedScene BrickGridScene;

	[Export] public int Rows = 4;
	[Export] public int Columns = 8;

	// Pixel size of brick png is 64x24 pixels
	[Export] public Vector2 BrickSize = new Vector2(64, 24);
	[Export] public Vector2 Spacing = new Vector2(4, 4);

	// Padding added to top of grid in viewport
	[Export] public int PaddingTopPixels = 50;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Width of bricks together with spacing between (cols - 1)
		var totalWidth = Columns * BrickSize.X + (Columns - 1) * Spacing.X;
		var viewportWidth = GetViewportRect().Size.X;
		// Height of bricks together with spacing between (rows - 1)
		var totalHeight = Rows * BrickSize.Y + (Rows - 1) * Spacing.Y;
		var viewportHeight = GetViewportRect().Size.Y;
		
		// Find where to start bricks x-position in middle of viewport.
		var startX = (viewportWidth - totalWidth) / 2f;

		if (totalHeight > viewportHeight || totalWidth > viewportWidth)
		{
			GD.PrintErr("Too many rows and/or columns were provided.");
		}
		
		for (var row = 0; row < Rows; row++)
		{
			for (var col = 0; col < Columns; col++)
			{
				var brick = (Node2D)BrickGridScene.Instantiate();
				brick.Position = new Vector2(
					startX + col * (BrickSize.X + Spacing.X), 
					PaddingTopPixels + row * (BrickSize.Y + Spacing.Y));
				AddChild(brick);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
