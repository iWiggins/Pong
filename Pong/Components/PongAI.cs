using GameFrame.Core.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Pong.Components;
/// <summary>
/// A component to control a paddle with math.
/// </summary>
/// <param name="ball">A reference to the ball.</param>
/// <param name="paddle">The paddle being controlled.</param>
/// <param name="field">A reference to the field.</param>
/// <param name="difficulty">Difficulty is just the maximum speed of the paddle.</param>
/// <remarks>
/// PongAI inherits Leaf because it is a component that has no children.
/// </remarks>
internal class PongAI(Ball ball, Paddle paddle, Field field, double difficulty) : Leaf, IUpdate
{
	public void Update(GameTime time)
	{
		// First determine what direction we are facing.
		// 1 we are facing left (the right paddle)
		// 2 we are facing right (the left paddle)
		int direction = paddle.Center.X > field.Center.X ? 1 : -1;

		// based on that, check if the ball is approaching us.
		bool comingThisWay = ball.Direction == direction;

		// Check if the ball is in the deadzone (on the other player's side of the field)
		bool inDeadzone = direction > 0 ?
		ball.Center.X < (int)(field.Left + field.Width * 0.5) :
		ball.Center.X > (int)(field.Left + field.Width * 0.5);

		// Only move if the ball is coming towards us and it is not on the other player's side.
		if(comingThisWay && !inDeadzone)
		{
			// First, calculate how far the ball is.
			int diff = direction > 0 ?
				paddle.Center.X - ball.Center.X:
				ball.Center.X - paddle.Center.X;
			// Then calculate the speed the palddle should move based on the difference
			// between the ball's Y and the paddle's Y, scaled up, and squared into a quadratic curve.
			double speed = Math.Abs((ball.Y - paddle.Y) * 1000 / (Math.Pow(diff, 2) + 100));

			// Put a cap on how fast the paddle can move (difficulty)
			paddle.Speed = Math.Max(difficulty, speed);

			// If the ball is below us, move down. Otherwise, move up.
			if(paddle.Center.Y < ball.Center.Y) paddle.MoveDown();
			else paddle.MoveUp();
		}
		else
		{
			// If the ball is out of range or moving away, stop paddle motion.
			paddle.Stop();
		}
	}
	
}
