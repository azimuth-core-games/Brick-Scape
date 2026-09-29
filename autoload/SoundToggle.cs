using System;
using Godot;

namespace brickpong2D.autoload;

/// <summary>
/// Sound toggle button. Registered as an autoload so it
/// stays in the corner of the viewport on every scene.
/// </summary>
public partial class SoundToggle : CanvasLayer
{
    [Export] private Button _button;

    public override void _Ready()
    {
        ValidateExports();

        // Keep keyboard input (arrows) going to the game, not this button
        _button.FocusMode = Control.FocusModeEnum.None;
        _button.Pressed += OnButtonPressed;
        _button.MouseEntered += OnButtonHovered;

        SoundManager.Instance.MutedChanged += UpdateButton;
        UpdateButton(SoundManager.Instance.Muted);
    }

    private void ValidateExports()
    {
        if (_button == null)
            throw new SystemException("_button is not set in SoundToggle.");
    }

    private void OnButtonHovered()
    {
        SoundManager.Instance.PlayUISfx(Sfx.ButtonHover);
    }

    private void OnButtonPressed()
    {
        SoundManager.Instance.ToggleMute();
        SoundManager.Instance.PlayUISfx(Sfx.ButtonClick);
    }

    private void UpdateButton(bool muted)
    {
        _button.Text = muted ? "SOUND: OFF" : "SOUND: ON";
    }

    public override void _ExitTree()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.MutedChanged -= UpdateButton;
    }
}
