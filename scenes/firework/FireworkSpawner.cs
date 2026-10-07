using System;
using brickpong2D.common;
using Godot;

namespace brickpong2D.scenes.firework;

public partial class FireworkSpawner : Node2D
{
    [Export] private PackedScene _fireworkScene;
    [Export] private Timer _spawnTimer;
    [Export] private float _minInterval = 0.3f;
    [Export] private float _maxInterval = 1.0f;
    [Export] private Vector2 _spawnMargin = new(64, 64); //Add margin to each firework spawn
    // Only spawn in the top portion of the screen (0.6 => top 60%)
    [Export] private float _maxSpawnHeightRatio = 0.6f;
    [Export] private bool _autoStart = true;

    private NodePool<FireworksParticle> _fireworkPool;

    public override void _Ready()
    {
        ValidateExports();
        // When firework Finished signal fires, release firework from pool.
        _fireworkPool = new NodePool<FireworksParticle>(
            _fireworkScene,
            this,
            onCreate: firework => firework.Finished += () => _fireworkPool.Release(firework)
            );

        _spawnTimer.OneShot = true;
        _spawnTimer.Timeout += OnSpawnTimerTimeout;
        if (_autoStart)
            Start();
    }

    public override void _ExitTree()
    {
        _spawnTimer.Timeout -= OnSpawnTimerTimeout;
    }

    private void ValidateExports()
    {
        if (_fireworkScene == null) throw new SystemException("_fireworkScene is not set in FireworkSpawner.");
        if (_spawnTimer == null) throw new SystemException("_spawnTimer is not set in FireworkSpawner.");
    }

    public void Start()
    {
        ScheduleNextSpawn();
    }

    public void Stop()
    {
        _spawnTimer.Stop();
        _fireworkPool.ReleaseAll();
    }

    private void OnSpawnTimerTimeout()
    {
        _fireworkPool.Acquire().Explode(RandomPosition());
        ScheduleNextSpawn();
    }

    private void ScheduleNextSpawn()
    {
        _spawnTimer.Start(GD.RandRange(_minInterval, _maxInterval));
    }

    private Vector2 RandomPosition()
    {
        var screen = GetViewportRect();

        var minX = screen.Position.X + _spawnMargin.X;
        var maxX = screen.End.X - _spawnMargin.X;
        var minY = screen.Position.Y + _spawnMargin.Y;
        var maxY = screen.Position.Y + screen.Size.Y * _maxSpawnHeightRatio;

        return new Vector2(
            (float)GD.RandRange(minX, maxX),
            (float)GD.RandRange(minY, maxY));
    }
}