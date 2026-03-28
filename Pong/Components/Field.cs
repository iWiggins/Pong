using GameFrame.Core.Geometrics;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using System;
using System.Collections.Generic;

namespace Pong.Components;
internal class Field : GeometricComponent, IReset
{
	public readonly Ball Ball;
	public readonly Paddle LeftPaddle;
	public readonly Paddle RightPaddle;

	public int Score { get; private set; } = 0;

	public delegate void ScoreChangedHandler(int oldScore, int newScore);
	public event ScoreChangedHandler? ScoreChanged;

	public Field(Keyboard keyboard, Mouse mouse, Texture2D ball, Texture2D paddle, SoundEffect ping, SoundEffect pong, PongSettings settings, Random rand)
	{
		Ball = new(this, ball, ping, pong, rand);
		Ball.BallBounced += CheckCollision;
		_children.Add(Ball);

		LeftPaddle = new(this, paddle, mouse)
		{
			Side = Wall.Left,
			Color = Palette.LeftPaddle
		};
		AddControls(LeftPaddle, keyboard, settings.LeftPaddle);
		_children.Add(LeftPaddle);
		

		RightPaddle = new(this, paddle, mouse)
		{
			Side = Wall.Right,
			Color = Palette.RightPaddle
		};
		AddControls(RightPaddle, keyboard, settings.RightPaddle);
		_children.Add(RightPaddle);		
	}

	public void Reset()
	{
		Score = 0;
	}

	public void Refresh()
	{
		Ball.Reset();
	}

	private void CheckCollision(Wall wall)
	{
		int oldScore = Score;
		if(Ball.Speed < 3) Ball.Speed *= 1.2;
		switch(wall)
		{
			case Wall.Left:
				Score -= 1;
				Refresh();
				break;
			case Wall.Right:
				Score += 1;
				Refresh();
				break;
			default:
				break;
		}
		if(ScoreChanged is not null && oldScore != Score) ScoreChanged(oldScore, Score);
	}

	public override IEnumerable<IComponent> Children => _children;

	public override bool HasChildren => true;

	public override bool AddChild(IComponent component) => false;
	public override void Invalidate() { }
	public override bool RemoveChild(IComponent component) => false;

	private void AddControls(Paddle paddle, Keyboard keyboard, Controller controller)
	{
		switch(controller)
		{
			case Controller.WASD:
				AddWASD(paddle, keyboard);
				break;
			case Controller.Arrows:
				AddArrows(paddle, keyboard);
				break;
			case Controller.Mouse:
				paddle.MouseControl = true;
				break;
			case Controller.CPU:
			default:
				AddCPU(paddle);
				break;
		}
	}

	private void AddWASD(Paddle paddle, Keyboard keyboard)
	{
		keyboard.KeyW.KeyPressed += k => paddle.MoveUp();
		keyboard.KeyW.KeyReleased += (k, d) => paddle.Stop();
		keyboard.KeyS.KeyPressed += k => paddle.MoveDown();
		keyboard.KeyS.KeyReleased += (k, d) => paddle.Stop();
	}

	private void AddArrows(Paddle paddle, Keyboard keyboard)
	{
		keyboard.KeyUp.KeyPressed += k => paddle.MoveUp();
		keyboard.KeyUp.KeyReleased += (k, d) => paddle.Stop();
		keyboard.KeyDown.KeyPressed += k => paddle.MoveDown();
		keyboard.KeyDown.KeyReleased += (k, d) => paddle.Stop();
	}

	private void AddCPU(Paddle paddle) =>
		_children.Add(new PongAI(Ball, paddle, this, 1.5));

	private readonly List<IComponent> _children = [];
}
