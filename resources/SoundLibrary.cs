using System;
using Godot;

namespace brickpong2D.resources;

[GlobalClass]
public partial class SoundLibrary : Resource
{
    [ExportGroup("SFX")] 
    [Export] public AudioStream BrickBreak { get; private set; }
    [Export] public AudioStream WallBounce { get; private set; }
    [Export] public AudioStream PaddleBounce { get; private set; }
    [Export] public AudioStream LifeLost { get; private set; }
    [Export] public AudioStream GameOver { get; private set; }
    [Export] public AudioStream Victory { get; private set; }
    
    [ExportGroup("UI")]
    [Export] public AudioStream ButtonClick { get; private set; }
    [Export] public AudioStream ButtonHover { get; private set; }
    
    [ExportGroup("Music")]
    [Export] public AudioStream MainMenuMusic { get; private set; }

    public void Validate()
    {
        if (BrickBreak == null)
            throw new SystemException("BrickBreak sound not set in SoundLibrary");
        if (WallBounce == null)
            throw new SystemException("WallBounce sound not set in SoundLibrary");
        if (PaddleBounce == null)
            throw new SystemException("PaddleBounce sound not set in SoundLibrary");
        if (LifeLost == null)
            throw new SystemException("LifeLost sound not set in SoundLibrary");
        if (GameOver == null)
            throw new SystemException("GameOver sound not set in SoundLibrary");
        if (ButtonClick == null)
            throw new SystemException("ButtonClick sound not set in SoundLibrary");
        if (ButtonHover == null)
            throw new SystemException("ButtonHover sound not set in SoundLibrary");
        if (Victory == null)
            throw new SystemException("Victory sound not set in SoundLibrary");
        if (MainMenuMusic == null)
            throw new SystemException("MainMenuMusic sound not set in SoundLibrary");
    }
}