using GameFrame.Core.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Pong.Components;
internal class PongAI(Ball ball, Paddle paddle, Field field, double difficulty) : Leaf, IUpdate
{
	public void Update(GameTime time)
	{
		int direction = paddle.Center.X > field.Center.X ? 1 : -1;

		bool comingThisWay = ball.Direction == direction;

		bool inDeadzone = direction > 0 ?
		ball.Center.X < (int)(field.Left + field.Width * 0.5) :
		ball.Center.X > (int)(field.Left + field.Width * 0.5);

		if(comingThisWay && !inDeadzone)
		{
			int diff = direction > 0 ?
				paddle.Center.X - ball.Center.X:
				ball.Center.X - paddle.Center.X;
			double speed = Math.Abs((ball.Y - paddle.Y) * 1000 / (Math.Pow(diff, 2) + 100));

			paddle.Speed = speed > difficulty ? difficulty : speed;

			if(paddle.Center.Y < ball.Center.Y) paddle.MoveDown();
			else paddle.MoveUp();
		}
		else
		{
			paddle.Stop();
		}
	}
	
}
