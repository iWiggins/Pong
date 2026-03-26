using GameFrame.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pong.Components;
internal class Ball(Field field, Texture2D texture, Random rand): Image(texture, field), IInitialize, IUpdate
{
	public const int BALL_SPEED_DIVISOR = 2000;
	public const int BALL_SCALE_DIVISOR = 20;

	public delegate void BallBounceHandler(Wall wall);
	public event BallBounceHandler BallBounced;

	public int Speed { get; private set; }

	/// <summary>
	/// The angle of the ball's movement in radians.
	/// </summary>
	public double Angle { get; private set; }

	/// <summary>
	/// Return the ball to starting position, reset speed, and randomize angle.
	/// </summary>
	public void Reset()
	{
		Speed = field.Geometry.Width / BALL_SPEED_DIVISOR;
		Angle = rand.NextDouble() * 2 * Math.PI;
		RecalculateVelocity();
		Height = field.Height / BALL_SCALE_DIVISOR;
		Width = Height;
		var c = field.Center;
		//X = c.X;
		//Y = c.Y;
		SetCenter(c.X, c.Y);
	}

	public void Initialize() => Reset();

	public void Update(GameTime time)
	{
		X += (int)(_xvelocity * time.ElapsedGameTime.TotalMilliseconds);
		Y += (int)(_yvelocity * time.ElapsedGameTime.TotalMilliseconds);

		if(_xvelocity > 0)
		{
			if(Geometry.Right > field.Geometry.Right) Bounce(Wall.Right);
		}
		else
		{
			if(Geometry.Left < field.Geometry.Left) Bounce(Wall.Left);
		}
		if(_yvelocity > 0)
		{
			if(Geometry.Bottom > field.Geometry.Bottom) Bounce(Wall.Botton);
		}
		else
		{
			if(Geometry.Top < field.Geometry.Top) Bounce(Wall.Top);
		}
	}

	public void Bounce(Wall wall)
	{
		switch(wall)
		{
			case Wall.Left:
			case Wall.Right:
				_xvelocity = -_xvelocity;
				break;
			case Wall.Top:
			case Wall.Botton:
				_yvelocity = -_yvelocity;
				break;
		}
		RecalculateAngle();
		if(BallBounced is not null) BallBounced(wall);
	}

	private void RecalculateVelocity()
	{
		_xvelocity = Math.Cos(Angle) * Speed;
		_yvelocity = Math.Tan(Angle) * Speed;
	}

	private void RecalculateAngle()
	{
		// There is a way to do this geometrically,
		// instead of trigonometrically.
		// TODO: Optimize this.
		Angle = Math.Asin(_yvelocity / _xvelocity);
	}

	private double _xvelocity;
	private double _yvelocity;
}
