using GameFrame.Components;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Pong.Components;
using System;

namespace Pong.Frames;
internal class PongCore(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds, Frame parent) : Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		Image background = new(resources.TextureBackground, null)
		{
			Layer = -1,
			Geometry = Screen.Bounds
		};
		Root.AddChild(background);

		FlowLayout downflow = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(downflow);

		FlowLayout topflow = new(FlowLayout.Direction.Right);
		downflow.AddChild(topflow);

		topflow.AddFiller(4);

		BoundText score = new(resources.FontScore)
		{
			Color = Color.White,
			Contents = "0"
		};
		topflow.AddChild(score);

		topflow.AddFiller(4);

		Field field = new(Keyboard, resources.TextureBall, resources.TexturePaddle, new());
		downflow.AddChild(field, 9);

		field.ScoreChanged += (oldScore, newScore) =>
		{
			if(newScore < 0)
			{
				score.Color = Color.Blue;
			}
			else if(newScore > 0)
			{
				score.Color = Color.Red;
			}
			else
			{
				score.Color = Color.White;
			}
			score.Contents = Math.Abs(newScore).ToString();
		};

		Keyboard.KeyEscape.KeyReleased += (k, d) => Exit(parent);
	}

	protected override void PostInitialize() => StartMusic();
	protected override void PostReset() => StartMusic();

	void StartMusic()
	{
		Song music = resources.SongGame;

		MediaPlayer.IsRepeating = true;
		if(MediaPlayer.State == MediaState.Playing)
		{
			MediaPlayer.Stop();
		}
		MediaPlayer.Play(music);
	}
}
