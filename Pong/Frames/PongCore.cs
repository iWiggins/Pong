using GameFrame.Components;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Components;
using System;

namespace Pong.Frames;
internal class PongCore(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds, Frame parent) : Frame(spriteBatch, bounds)
{
	BoundText score;

	protected override void PreInitialize()  
	{
		Image background = new(resources.TextureBackground, null, -1)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(background);
		FlowLayout downflow = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(downflow);
		{
			FlowLayout topflow = new(FlowLayout.Direction.Right)
			{
				Geometry = Screen.Bounds
			};
			downflow.AddChild(topflow);
			{
				topflow.AddFiller(4);
				score = new(resources.FontScore)
				{
					Color = Color.White,
					Contents = "0"
				};
				topflow.AddChild(score);
				topflow.AddFiller(4);
			}
			Field field = new(Keyboard, resources.TextureBall, resources.TexturePaddle, new())
			{
				Geometry = Screen.Bounds
			};
			downflow.AddChild(field, 9);
			field.ScoreChanged += UpdateScore;
		}
		Keyboard.KeyEscape.KeyReleased += (k, d) => Exit(parent);
	}

	private void UpdateScore(int oldScore, int newScore)
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
	}
}
