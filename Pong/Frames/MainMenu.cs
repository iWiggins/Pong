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
/// <summary>
/// The main menu for Pong
/// </summary>
/// <param name="resources">An instance of the Pong resource manager.</param>
/// <param name="spriteBatch">The sprite batch used for the game.</param>
/// <param name="bounds">A bounds provider for the screen size.</param>
/// <param name="settings">Pong settings.</param>
internal class MainMenu(
	ResourceManager resources,
	SpriteBatch spriteBatch,
	IBoundsProvider bounds,
	PongSettings settings):
	Frame(spriteBatch, bounds)
{
	public PongSettings Settings { get; } = settings;
	protected override void PreInitialize()
	{
		// This logic runs before the game begins.

		// First, create a custom cursor for use in the menus.
		int cursorSize = Screen.Bounds.Height / 50;
		Mouse.Cursor = new MouseCursor(resources.TextureBall)
		{
			Width = cursorSize,
			Height = cursorSize
		};

		// Then create a downward flow to arrange the elements on the screen.
		// Its geometry is equal to the screen size.
		FlowLayout outerLayout = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(outerLayout); // Add the layout as the child of the root element.

		// Add a filler so there is some space at the top of the screen.
		outerLayout.AddFiller();

		// Create a fill layout to stretch the background and main layout
		// to fill the space provided by the outer layout.
		FillLayout stack = new();
		outerLayout.AddChild(stack, 8);

		// More filler after the stack, so there is space at the bottom of the screen.
		outerLayout.AddFiller();

		// Create a background image and add it to the stack.
		Image background = new(resources.TextureBackground, null);
		stack.AddChild(background);

		// Create another downward flow to organize the title and buttons.
		FlowLayout mainLayout = new(FlowLayout.Direction.Down);
		stack.AddChild(mainLayout);

		// Create a rightward flow to position the title
		FlowLayout topLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(topLayout, 4);

		// Add filler so there is empty space on the left of the title
		topLayout.AddFiller(4);

		// Add the title itself.
		BoundText title = new(resources.FontTitle)
		{
			Contents = "Pong",
			Color = Palette.Neutral
		};
		topLayout.AddChild(title, 4);

		// More filler so there is empty space on the right of the title.
		topLayout.AddFiller(4);

		// Filler in the main layout so there is space between the title and buttons.
		mainLayout.AddFiller();

		// Create a righward flow to position the buttons.
		FlowLayout buttonLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(buttonLayout, 2);
		
		// Filler on the left side of the buttons.
		buttonLayout.AddFiller(1);

		// Add the play button.
		HoverTextImageButton playButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Play",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		// Create an event handler so when the play button is pressed, we exit to the PongCore frame.
		playButton.Released += (b, p, dt) => Exit(new PongCore(
			resources,
			SpriteBatch,
			Screen,
			Mouse.Cursor,
			this,
			Settings));
		buttonLayout.AddChild(playButton, 2);

		// Filler on the right side of the buttons.
		buttonLayout.AddFiller(1);

		// Add the options button.
		HoverTextImageButton optionsButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Options",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		// Create an event handler so when the options button is pressed, we exit to the OptionsMenu frame.
		optionsButton.Released += (b, p, dt) => Exit(new OptionsMenu(
			Mouse.Cursor,
			resources,
			SpriteBatch,
			Screen,
			Settings,
			this));
		buttonLayout.AddChild(optionsButton, 2);

		// Filler for space between play and options.
		buttonLayout.AddFiller(1);

		// Add the quit button
		HoverTextImageButton quitButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Quit",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		// Create an event handler so when the quit button is pressed, we exit the game (move to null).
		quitButton.Released += (b, p, dt) => Exit(null);
		buttonLayout.AddChild(quitButton, 2);

		// Filler for empty space on the right side of the buttons.
		buttonLayout.AddFiller(1);
	}

	// After the initialization completes, the music should start.
	protected override void PostInitialize() => StartMusic();
	protected override void PostReset()
	{
		// Ensure the mouse shows back up when we switch from the main game
		// (which disables the custom cursor)
		// back to the menu.
		if(Mouse.Cursor is not null) Mouse.Cursor.Enabled = true;
		// Then restart the menu music.
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
		// Our volume setting is an integer from 1-10,
		// but the media player needs a float from 0 to 1.
		// So we scale our int down to the right range.
		MediaPlayer.Volume = Settings.Volume / 10.0f;
		MediaPlayer.Play(music);
	}
}
