using GameFrame.Components;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Pong.Components;
using Pong.Data;
using System;

namespace Pong.Frames;
/// <summary>
/// The main Pong gameplay.
/// </summary>
/// <param name="resources">The Pong resource manager</param>
/// <param name="spriteBatch">The game's spritebatch</param>
/// <param name="bounds">The boundry provider from the main menu</param>
/// <param name="cursor">The custom cursor we are using in the main menu.</param>
/// <param name="settings">The pong settings file from the main menu</param>
/// <param name="parent">A reference to the main menu so we can return to it without creating a new object.</param>
internal class PongCore(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds, IMouseCursor cursor, Frame parent, PongSettings settings) : Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		// Set the custom cursor, then disable it.
		// This makes the mouse invisible when we are playing the game.
		Mouse.Cursor = cursor;
		cursor.Enabled = false;

		// Create a downward flow to organize the score row and main game field.
		FlowLayout downflow = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(downflow);

		// Now a horizontal flow to position the score.
		FlowLayout topflow = new(FlowLayout.Direction.Right);
		downflow.AddChild(topflow, 1);

		// Empty space left of the score
		topflow.AddFiller(4);

		// Create a BoundText for the score, so it will resize to fill the available space.
		BoundText score = new(resources.FontNumber)
		{
			Color = Palette.Neutral,
			Contents = "0"
		};
		topflow.AddChild(score);

		// Empty space right of the score.
		topflow.AddFiller(4);

		// A fill layout to stack the game field on top of the background.
		FillLayout stack = new();
		downflow.AddChild(stack, 8);

		// The background image, added to the stack
		Image background = new(resources.TextureBackground)
		{
			Layer = -1,
		};
		stack.AddChild(background);

		// The main game field.
		Field field = new(
			Keyboard,
			Mouse,
			resources.TextureBall,
			resources.TexturePaddle,
			resources.SfxPing,
			resources.SfxPong,
			settings,
			new());
		stack.AddChild(field);

		// An event handler so when the game's score changes,
		// we change the text displaying the score.
		field.ScoreChanged += (oldScore, newScore) =>
		{
			if(newScore < 0)
			{
				score.Color = Palette.RightPaddle;
			}
			else if(newScore > 0)
			{
				score.Color = Palette.LeftPaddle;
			}
			else
			{
				score.Color = Palette.Neutral;
			}
			score.Contents = Math.Abs(newScore).ToString();
		};

		// Empty space at the bottom of the screen.
		downflow.AddFiller(1);

		// A keyboard event handler so when the escape key is pressed,
		// we return to the main menu.
		Keyboard.KeyEscape.KeyReleased += (k, d) => Exit(parent);
	}

	// After other components initialize, start the game music.
	protected override void PostInitialize() => StartMusic();

	// If we return to this frame from another,
	// disable the mouse again and restart the music.
	protected override void PostReset()
	{
		cursor.Enabled = false;
		StartMusic();
	}

	void StartMusic()
	{
		Song music = resources.SongGame;

		MediaPlayer.IsRepeating = true;
		if(MediaPlayer.State == MediaState.Playing)
		{
			MediaPlayer.Stop();
		}
		MediaPlayer.Volume = settings.Volume / 10.0f;
		MediaPlayer.Play(music);
	}
}
