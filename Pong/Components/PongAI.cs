using GameFrame.Core.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Pong.Components;
internal class PongAI(Ball ball, Paddle paddle, Field field, double difficulty) : Leaf, IUpdate
{
	public void Update(GameTime time)
	{
		// AI is always the right paddle. TODO: Genericize the math.

		if(ball.Center.X < field.Center.X)
		{
			paddle.Stop();
		}
		else
		{
			/*int delta = paddle.Left - ball.Right;
			double displacement = field.Width/2 / delta;

			paddle.Speed = displacement / 2;*/

			double speed = Math.Abs((ball.Y - paddle.Y) * 1000 / (Math.Pow(ball.X - paddle.X, 2) + 100));

			paddle.Speed = speed > difficulty ? difficulty : speed;

			if(paddle.Center.Y < ball.Center.Y) paddle.MoveDown();
			else paddle.MoveUp();
		}

			

		


	}
}
