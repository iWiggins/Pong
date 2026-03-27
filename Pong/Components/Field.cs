using GameFrame.Core.Geometrics;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
	public event ScoreChangedHandler ScoreChanged;

	public Field(Keyboard keyboard, Texture2D ball, Texture2D paddle, Random rand)
	{
		Ball = new(this, ball, rand);
		Ball.BallBounced += CheckCollision;

		LeftPaddle = new(this, paddle)
		{
			Side = Wall.Left,
			Color = Color.Red
		};
		keyboard.KeyW.KeyPressed += k => LeftPaddle.MoveUp();
		keyboard.KeyW.KeyReleased += (k, d) => LeftPaddle.Stop();
		keyboard.KeyS.KeyPressed += k => LeftPaddle.MoveDown();
		keyboard.KeyS.KeyReleased += (k, d) => LeftPaddle.Stop();

		RightPaddle = new(this, paddle)
		{
			Side = Wall.Right,
			Color = Color.Blue
		};
		keyboard.KeyUp.KeyPressed += k => RightPaddle.MoveUp();
		keyboard.KeyUp.KeyReleased += (k, d) => RightPaddle.Stop();
		keyboard.KeyDown.KeyPressed += k => RightPaddle.MoveDown();
		keyboard.KeyDown.KeyReleased += (k, d) => RightPaddle.Stop();
	}

	public void Reset()
	{
		Score = 0;
	}

	public void Refresh()
	{
		Ball.Reset();
		LeftPaddle.Reset();
		RightPaddle.Reset();
	}

	private void CheckCollision(Wall wall)
	{
		int oldScore = Score;
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

	public override IEnumerable<IComponent> Children => [Ball, LeftPaddle, RightPaddle];

	public override bool HasChildren => true;

	public override bool AddChild(IComponent component) => false;
	public override void Invalidate() { }
	public override bool RemoveChild(IComponent component) => false;
}
