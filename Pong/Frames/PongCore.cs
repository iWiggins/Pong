using GameFrame.Components;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Pong.Components;
using Pong.Data;
using System;

namespace Pong.Frames;
internal class PongCore(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds, IMouseCursor cursor, Frame parent, PongSettings settings) : Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		Mouse.Cursor = cursor;
		cursor.Enabled = false;
		FlowLayout downflow = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(downflow);

		FlowLayout topflow = new(FlowLayout.Direction.Right);
		downflow.AddChild(topflow, 1);

		topflow.AddFiller(4);

		BoundText score = new(resources.FontNumber)
		{
			Color = Palette.Neutral,
			Contents = "0"
		};
		topflow.AddChild(score);

		topflow.AddFiller(4);

		FillLayout stack = new();
		downflow.AddChild(stack, 8);

		Image background = new(resources.TextureBackground)
		{
			Layer = -1,
		};
		stack.AddChild(background);

		Field field = new(Keyboard, resources.TextureBall, resources.TexturePaddle, resources.SfxPing, resources.SfxPong, settings, new());
		stack.AddChild(field);

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

		downflow.AddFiller(1);

		Keyboard.KeyEscape.KeyReleased += (k, d) => Exit(parent);
	}

	protected override void PostInitialize() => StartMusic();
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
