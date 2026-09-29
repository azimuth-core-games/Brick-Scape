using System.Collections.Generic;
using Godot;

namespace brickpong2D.scenes.hud.lives;

public partial class LivesDisplay : HBoxContainer
{
	private readonly List<TextureRect> _lives = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var child in GetChildren())
		{
			if (child is TextureRect life)
			{
				_lives.Add(life);
			}
		}
	}

	public void ShowLives(int lives)
	{
		for (int i = 0; i < _lives.Count; i++)
		{
			_lives[i].Visible = i < lives;
		}
	}
}