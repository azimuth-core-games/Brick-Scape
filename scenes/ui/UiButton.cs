using brickpong2D.autoload;
using Godot;

namespace brickpong2D.scenes.ui;

/// <summary>
/// Button that signals when hovered/pressed to the EventBus
/// </summary>
[GlobalClass]
public partial class UiButton : Button
{
	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		Pressed += OnPressed;
	}

	private void OnMouseEntered() => 
		EventBus.Instance.EmitSignal(EventBus.SignalName.ButtonHovered);
	private void OnPressed() => 
		EventBus.Instance.EmitSignal(EventBus.SignalName.ButtonPressed);

	public override void _ExitTree()
	{
		MouseEntered -= OnMouseEntered;
		Pressed -= OnPressed;
	}
}