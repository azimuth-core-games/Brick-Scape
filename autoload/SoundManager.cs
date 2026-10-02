using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace brickpong2D.autoload;

public partial class SoundManager : Node
{
	public static SoundManager Instance { get; private set; }

	[Signal]
	public delegate void MutedChangedEventHandler(bool muted);

	// Master bus is always index 0
	private const int MasterBusIndex = 0;
	public bool Muted => AudioServer.IsBusMute(MasterBusIndex);

	private int _sfxPlayerCount = 8;
	private AudioStreamPlayer _musicPlayer;
	private AudioStreamPlayer _uiPlayer;
	private readonly Queue<AudioStreamPlayer2D> _sfxPlayers = new();

	public override void _Ready()
	{
		Instance = this;

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
	}
	
	public void PlayUISfx(AudioStream stream)
	{
		_uiPlayer.Stream = stream;
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
	
	// default sets position to (0,0) if not provided.
	public void PlaySfx(AudioStream stream, Vector2 position = default)
	{
		var player = _sfxPlayers.Dequeue();
		player.Stream = stream;
		player.Position = position;
		player.Play();
		
		_sfxPlayers.Enqueue(player);
	}

	public void ToggleMute()
	{
		AudioServer.SetBusMute(MasterBusIndex, !Muted);
		EmitSignal(SignalName.MutedChanged, Muted);
	}

	public override void _ExitTree()
	{
		if (Instance == this)
			Instance = null;
	}
}
