using Godot;
using System;

public partial class BrickGrid : Node2D
{
	[Export] public PackedScene BrickGridScene;

	[Export] public int Rows = 4;
	[Export] public int Columns = 8;

	[Export] public Vector2 BrickSize = new Vector2(64, 24);
	[Export] public Vector2 Spacing = new Vector2(4, 4);

	[Export] public Vector2 GridOffset = new Vector2(50, 50);
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var totalWidth = Columns * BrickSize.X + (Columns - 1) * Spacing.X;
		var viewportWidth = GetViewportRect().Size.X;
		var startX = (viewportWidth - totalWidth) / 2f;
		
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				var brick = (Node2D)BrickGridScene.Instantiate();
				brick.Position = GridOffset + new Vector2(
					startX + col * (BrickSize.X + Spacing.X), 
					GridOffset.Y + row * (BrickSize.Y + Spacing.Y));
				AddChild(brick);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
