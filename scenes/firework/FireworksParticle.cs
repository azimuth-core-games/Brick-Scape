using System;
using brickpong2D.common;
using Godot;

namespace brickpong2D.scenes.firework;

public partial class FireworksParticle : CpuParticles2D, IPoolable
{
    [Export] private Color[] _baseColors = [];
   
    public override void _Ready()
    {
        ValidateExports();
    }

    public void OnAcquire() => Visible = true;

    public void OnRelease()
    {
        Emitting = false;
        Visible = false;
    }

    public void Explode(Vector2 globalPosition)
    {
        GlobalPosition = globalPosition;
        // Choose random base color for each firework
        Color = _baseColors[GD.RandRange(0, _baseColors.Length - 1)];
        Restart();
    }

    private void ValidateExports()
    {
        if (_baseColors == null || _baseColors.Length == 0)
            throw new SystemException("_baseColors is not set in FireworksParticle.");
    }
}