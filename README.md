# Brick Scape
A nostalgic 2D brick pong style game. Break all the bricks by bouncing the ball on the paddle.  

<p>
	<img src="docs/screenshots/brick-scape-main-menu.png" width="400" alt="Main menu" />
	<img src="docs/screenshots/brick-scape-gameplay.gif" width="400" alt="Gameplay" />
</p>

## Features  
- 112 bricks to clear (7 rows × 16 columns)  
- Paddle bounce angle depends on where the ball hits the paddle  
- Score tracking and 3 lives, shown as hearts  
- Victory screen with fireworks  
- Retro arcade style music and sound effects, with a sound on/off toggle  
- Custom pixel art made with Aseprite

## Controls  
| Action            | Input         |
| ----------------- | ------------- |
| Move paddle left  | ← Left arrow  |
| Move paddle right | → Right arrow |
| Menus             | Mouse or Tab  |
| Sound Toggle      | Mouse         |
  
## Project Structure  
```  
autoload/     Global singletons (EventBus, GameManager, SceneManager, SoundManager…)  
common/       Shared code such as the generic NodePool  
resources/    Custom resources (SoundLibrary, UI Theme)  
scenes/       Game scenes and their scripts (ball, brick, paddle, hud, levels…)  
assets/       Art, audio and fonts  
```  
  
## Tools  
- Godot 4.7 (.NET / C#)
- Aseprite

## Download and Play
Download and play on [itch.io](https://alishcodes.itch.io/brick-scape) for Windows, macOS, and Linux systems.

## Building from Source
Requirements:
- [Godot 4.7 .NET edition](https://godotengine.org/download) (must use the version to run C#)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

1. Clone the repo
2. Open or Import `project.godot` from the cloned `Brick-Scape` folder in Godot 4.7 .NET
3. Run the game with `F5` if using Windows or Linux and `Command + B` if using macOS
4. To export the project select **Project -> Export** with the desired export templates (Windows, Linux, macOS, etc)

## Credits  
- **Font:** [Minecraft](https://www.dafont.com/minecraft.font)  by Craftron Gaming
- **Music:** [Neon Gaming](https://pixabay.com/music/synthwave-neon-gaming-128925/) by dopestuff
- **Sound effects:** 
	- [Click Button](https://pixabay.com/sound-effects/film-special-effects-click-button-140881/) by Universfield
	- [Minimalist Button Hover](https://pixabay.com/sound-effects/film-special-effects-minimalist-button-hover-sound-effect-399749/) by Lesiakower
	- [Game Over](https://pixabay.com/sound-effects/technology-game-over-retro-8bit-sfx-499656/) by DRAGON-STUDIO
	- [Victory Bell Success Fanfare](https://pixabay.com/sound-effects/musical-victory-bell-success-fanfare-576275/) by Emand_Edroff
	- [Arcade FX](https://pixabay.com/sound-effects/film-special-effects-arcade-fx-288597/) by lucadialessandro
	- [Metal Slam 2](https://pixabay.com/sound-effects/film-special-effects-metal-slam-2-189784/) by floraphonic
	- [ballhit](https://pixabay.com/sound-effects/film-special-effects-ballhit-352569/) by u_p2u1nij0ok
	- [small explosion](https://pixabay.com/sound-effects/film-special-effects-small-explosion-103779/) by bevibeldesign (Freesound)
- Pixel art: [alishcodes](https://github.com/alishcodes)

## License
- Code is licensed under the MIT License (see `LICENSE`).
- Pixel art (`assets/art/`): [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/), public domain, free to use for any purpose without attribution.
- Music and SFX: Pixabay (see individual creators above), [Pixabay Content License](https://pixabay.com/service/license-summary/)
- Font: Minecraft.ttf (see creator above), [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/deed.en), Creative Commons Attribution 4.0
