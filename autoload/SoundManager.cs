using System.Collections;
using System.Collections.Generic;
using Godot;

namespace brickpong2D.autoload;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    private int _sfxPlayerCount = 8;
    private AudioStreamPlayer _musicPlayer;
    private readonly Queue<AudioStreamPlayer> _sfxPlayers = new();

    public override void _Ready()
    {
        Instance = this;

        _musicPlayer = new AudioStreamPlayer { Bus = "Music" };
        AddChild(_musicPlayer);

        // add multiple players for SFX 
        for (int i = 0; i < _sfxPlayerCount; i++)
        {
            var player = new AudioStreamPlayer { Bus = "SFX" };
            AddChild(player);
            _sfxPlayers.Enqueue(player);
        }
    }

    public void PlayMusic(AudioStream stream, bool loop = true)
    {
        // same audio is already being played
        if (_musicPlayer.Stream == stream && _musicPlayer.Playing) return;

        switch (stream)
        {
            // set audio to loop for ogg and wav (looping mp3 not supported)
            case AudioStreamOggVorbis ogg:
                ogg.Loop = loop;
                break;
            case AudioStreamWav wav:
                // if loop is true, set audio to loop forward.
                wav.LoopMode = loop
                    ? AudioStreamWav.LoopModeEnum.Forward
                    : AudioStreamWav.LoopModeEnum.Disabled;
                break;
        }

        _musicPlayer.Stream = stream;
        _musicPlayer.Play();
    }
    
    public void StopMusic() => _musicPlayer.Stop();
    
    // volume in decibels
    public void PlaySfx(AudioStream stream, float volumeDb = 0f)
    {
        var player = _sfxPlayers.Dequeue();
        player.Stream = stream;
        player.VolumeDb = volumeDb;
        player.Play();
        
        _sfxPlayers.Enqueue(player);
    }
}