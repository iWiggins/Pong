using GameFrame.Components;
using GameFrame.Components.Buttons;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Pong.Data;

namespace Pong.Frames;
internal class MainMenu(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds) : Frame(spriteBatch, bounds)
{
	public readonly PongSettings Settings = new()
	{
		LeftPaddle = Controller.WASD,
		RightPaddle = Controller.CPU,
		Volume = 10
	};

	protected override void PreInitialize()
	{
		int cursorSize = Screen.Bounds.Height / 50;
		Mouse.Cursor = new MouseCursor(resources.TextureBall)
		{
			Width = cursorSize,
			Height = cursorSize
		};

		FlowLayout outerLayout = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(outerLayout);

		outerLayout.AddFiller();

		FillLayout stack = new();
		outerLayout.AddChild(stack, 8);

		outerLayout.AddFiller();

		Image background = new(resources.TextureBackground, null);
		stack.AddChild(background);

		FlowLayout mainLayout = new(FlowLayout.Direction.Down);
		stack.AddChild(mainLayout);

		FlowLayout topLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(topLayout, 4);
		topLayout.AddFiller(4);

		BoundText title = new(resources.FontTitle)
		{
			Contents = "Pong",
			Color = Palette.Neutral
		};
		topLayout.AddChild(title, 4);
		topLayout.AddFiller(4);

		mainLayout.AddFiller();

		FlowLayout buttonLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(buttonLayout, 2);
		
		buttonLayout.AddFiller(1);

		HoverTextImageButton playButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Play",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		playButton.Released += (b, p, dt) => Exit(new PongCore(resources, SpriteBatch, Screen, Mouse.Cursor, this, Settings));
		buttonLayout.AddChild(playButton, 2);

		buttonLayout.AddFiller(1);

		HoverTextImageButton optionsButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Options",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		optionsButton.Released += (b, p, dt) => Exit(new OptionsMenu(Mouse.Cursor, resources, SpriteBatch, Screen, Settings, this));
		buttonLayout.AddChild(optionsButton, 2);

		buttonLayout.AddFiller(1);

		HoverTextImageButton quitButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Quit",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		quitButton.Released += (b, p, dt) => Exit(null);
		buttonLayout.AddChild(quitButton, 2);

		buttonLayout.AddFiller(1);

	}

	protected override void PostInitialize() => StartMusic();
	protected override void PostReset()
	{
		if(Mouse.Cursor is not null) Mouse.Cursor.Enabled = true;
		StartMusic();
	}

	void StartMusic()
	{
		Song music = resources.SongMenu;

		MediaPlayer.IsRepeating = true;
		if(MediaPlayer.State == MediaState.Playing)
		{
			MediaPlayer.Stop();
		}
		MediaPlayer.Volume = Settings.Volume / 10.0f;
		MediaPlayer.Play(music);
	}
}
