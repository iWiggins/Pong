using GameFrame.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pong.Components;
internal class Ball(Field field, Texture2D texture, Random rand) : Image(texture), IInitialize, IUpdate, IReset
{
	public double Speed { get; set; } = 1;
	public double Acceleration { get; set; } = 1;
	public double Scale { get; set; } = 1;
	public double Deviance { get; set; } = 0.2;

	public delegate void BallBounceHandler(Wall wall);
	public event BallBounceHandler BallBounced;

	public bool Initialized { get; private set; } = false;

	/// <summary>
	/// Return the ball to starting position, reset speed, and randomize angle.
	/// </summary>
	public void Reset()
	{
		double speed = field.Geometry.Width  * Speed / 5000;

		int direction = rand.NextDouble() > 0.5 ? 1 : -1;

		double xVector = speed * rand.NextDouble();
		double yVector = 1 - xVector;

		_xvelocity = speed * xVector * direction;
		_yvelocity = speed * yVector;

		
		Height = (int)(field.Height * Scale / 20);
		Width = Height;
		SetCenter(field.Center);
	}

	public void Initialize()
	{
		Reset();
		Initialized = true;
	}

	public void Update(GameTime time)
	{
		X += (int)(_xvelocity * time.ElapsedGameTime.TotalMilliseconds);
		Y += (int)(_yvelocity * time.ElapsedGameTime.TotalMilliseconds);

		if(_xvelocity > 0)
		{
			_xvelocity += Acceleration / 1000;
			if(HitRightPaddle) Bounce(Wall.Right, false);
			else if(HitRightWall) Bounce(Wall.Right, true);
		}
		else
		{
			_xvelocity -= Acceleration / 1000;
			if(HitLeftPaddle) Bounce(Wall.Left, false);
			else if(HitLeftWall) Bounce(Wall.Left, true);
		}
		if(_yvelocity > 0)
		{
			_yvelocity += Acceleration / 1000;
			if(Geometry.Bottom > field.Geometry.Bottom) Bounce(Wall.Botton, false);
		}
		else
		{
			_yvelocity -= Acceleration / 1000;
			if(Geometry.Top < field.Geometry.Top) Bounce(Wall.Top, false);
		}
	}

	public void Bounce(Wall wall, bool raiseEvent = false)
	{
		double deviation = (rand.NextDouble() - 0.5) * Deviance;
		double newXvelocity = _xvelocity + _xvelocity * deviation;
		double newYVelocity = _yvelocity - _yvelocity * deviation;
		
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
		if(raiseEvent && BallBounced is not null) BallBounced(wall);
	}

	private bool HitLeftWall => Left < field.Left;
	private bool HitRightWall => Right > field.Right;

	private bool HitLeftPaddle =>
		Left < field.LeftPaddle.Right && Top < field.LeftPaddle.Bottom && Bottom > field.LeftPaddle.Top;
	private bool HitRightPaddle =>
		Right > field.RightPaddle.Left && Top < field.RightPaddle.Bottom && Bottom > field.RightPaddle.Top;



	private double _xvelocity;
	private double _yvelocity;
}
