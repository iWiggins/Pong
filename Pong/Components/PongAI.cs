using GameFrame.Core.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Pong.Components;
internal class PongAI(Ball ball, Paddle paddle, Field field, double difficulty) : Leaf, IUpdate
{
	int direction = paddle.X > field.X ? 1 : -1;
	public void Update(GameTime time)
	{
		if(BallInDeadzone)
		{
			paddle.Stop();
		}
		else
		{
			int diff = direction > 0 ?
				ball.X - paddle.X :
				paddle.X - ball.X;
			double speed = Math.Abs((ball.Y - paddle.Y) * 1000 / (Math.Pow(diff, 2) + 100));

			paddle.Speed = speed > difficulty ? difficulty : speed;

			if(paddle.Center.Y < ball.Center.Y) paddle.MoveDown();
			else paddle.MoveUp();
		}
	}

	bool BallInDeadzone => direction > 0 ?
		ball.Center.X < (int)(field.Center.X * 1.2) :
		ball.Center.X > (int)(field.Center.X * 0.8);
}
