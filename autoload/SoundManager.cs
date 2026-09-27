using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace brickpong2D.autoload;

public enum Sfx { BrickBreak, WallBounce, PaddleBounce, ButtonClick, ButtonHover }

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    private int _sfxPlayerCount = 8;
    private AudioStreamPlayer _musicPlayer;
    private AudioStreamPlayer _uiPlayer;
    private readonly Queue<AudioStreamPlayer2D> _sfxPlayers = new();

    // Sound effects
    [Export] private AudioStream _brickBreak;
    [Export] private AudioStream _wallBounce;
    [Export] private AudioStream _paddleBounce;
    [Export] private AudioStream _buttonClick;
    [Export] private AudioStream _buttonHover;
    
    // Sound effect library
    private Dictionary<Sfx, AudioStream> _sfxLibrary;

    public override void _Ready()
    {
        Instance = this;
        ValidateExports();

        _musicPlayer = new AudioStreamPlayer { Bus = "Music" };
        AddChild(_musicPlayer);
        
        _uiPlayer = new AudioStreamPlayer { Bus = "UI" };
        AddChild(_uiPlayer);

        // add multiple players for SFX 
        for (int i = 0; i < _sfxPlayerCount; i++)
        {
            var player = new AudioStreamPlayer2D { Bus = "SFX" };
            AddChild(player);
            _sfxPlayers.Enqueue(player);
        }

        _sfxLibrary = new()
        {
            { Sfx.BrickBreak, _brickBreak },
            { Sfx.WallBounce, _wallBounce },
            { Sfx.PaddleBounce, _paddleBounce },
            { Sfx.ButtonClick, _buttonClick },
            { Sfx.ButtonHover, _buttonHover },
        };
    }

    private void ValidateExports()
    {
        if (_brickBreak == null)
            throw new SystemException("_brickBreak is not set in SoundManager.");
        if (_wallBounce == null)
            throw new SystemException("_wallBounce is not set in SoundManager.");
        if (_paddleBounce == null)
            throw new SystemException("_paddleBounce is not set in SoundManager.");
        if (_buttonClick == null)
            throw new SystemException("_buttonClick is not set in SoundManager.");
        if (_buttonHover == null)
            throw new SystemException("_buttonHover is not set in SoundManager.");
    }

    public void PlayUISfx(Sfx sfx)
    {
        _uiPlayer.Stream = _sfxLibrary[sfx];
        _uiPlayer.Play();
    }

    public void PlayMusic(AudioStream stream, bool loop = true)
    {
        // same audio is already being played
        if (_musicPlayer.Stream == stream && _musicPlayer.Playing) return;

        switch (stream)
        {
            // set audio to loop
            case AudioStreamOggVorbis ogg:
                ogg.Loop = loop;
                break;
            case AudioStreamWav wav:
                // if loop is true, set audio to loop forward.
                wav.LoopMode = loop
                    ? AudioStreamWav.LoopModeEnum.Forward
                    : AudioStreamWav.LoopModeEnum.Disabled;
                break;
            case AudioStreamMP3 mp3:
                mp3.Loop = loop;
                break;
        }

        _musicPlayer.Stream = stream;
        _musicPlayer.Play();
    }
    
    public void StopMusic() => _musicPlayer.Stop();
    
    // volume in decibels
    public void PlaySfx(Sfx sfx, Vector2? position = null)
    {
        GD.Print($"SOUNDMANAGER --- Now playing sfx {sfx} at {position}.");
        var player = _sfxPlayers.Dequeue();
        player.Stream = _sfxLibrary[sfx];
        if (position != null)
        {
            player.Position = position.Value;
        }

        player.Play();
        
        _sfxPlayers.Enqueue(player);
    }
}