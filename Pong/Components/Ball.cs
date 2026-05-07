using GameFrame.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using System;

namespace Pong.Components;
/// <summary>
/// A component that tracks the ball, implementing the collision logic and its drawing.
/// </summary>
/// <param name="field">A reference to the field this ball belongs to.</param>
/// <param name="texture">The texture used for the ball's image.</param>
/// <param name="ping">A sound effect for bouncing off walls.</param>
/// <param name="pong">A sound effect for scores.</param>
/// <param name="rand">A random number generator.</param>
/// <remarks>
/// Ball inherits Image because it needs to be visible on screen,
/// and it doesn't have any further children.
/// </remarks>
internal class Ball(Field field, Texture2D texture, SoundEffect ping, SoundEffect pong, Random rand) : Image(texture), IInitialize, IUpdate, IReset
{
	/// <summary>
	/// How fast the ball should start moving at.
	/// </summary>
	public double Speed { get; set; } = 1;
	/// <summary>
	/// How much the ball should accelerate as it moves.
	/// </summary>
	public double Acceleration { get; set; } = 1;
	/// <summary>
	/// How large the ball should be.
	/// </summary>
	public double Scale { get; set; } = 1;
	/// <summary>
	/// How much randomness in the deflection angle when the ball bounces.
	/// </summary>
	public double Deviance { get; set; } = 0.2;
	/// <summary>
	/// The volume of the ball's sound effects.
	/// </summary>
	public int Volume { get; set; } = 10;

	/// <summary>
	/// Handles the ball hitting one of the back walls.
	/// </summary>
	/// <param name="wall">Which wall was hit.</param>
	public delegate void BallBounceHandler(Wall wall);

	/// <summary>
	/// Raised when the ball hits one of the back walls.
	/// </summary>
	public event BallBounceHandler? BallBounced;

	public bool Initialized { get; private set; } = false;

	/// <summary>
	/// Indicates whether the ball is moving right (1) or left (-1).
	/// </summary>
	public int Direction => _xvelocity > 0 ? 1 : -1;

	/// <summary>
	/// Return the ball to starting position, reset speed, and randomize angle.
	/// </summary>
	public void Reset()
	{
		// Speed is based on the setting and how wide the field is.
		double speed = field.Width  * Speed / 5000;

		// Randomize which direction the ball goes in.
		int direction = rand.NextDouble() > 0.5 ? 1 : -1;

		// Randomize how much of the ball's speed is converted to X velocity.
		double xVector = rand.NextDouble();
		// Y velocity is the remaining speed.
		double yVector = 1 - xVector;

		// Calculate the velocities from the speed and the vector multipliers.
		_xvelocity = speed * xVector * direction;
		_yvelocity = speed * yVector;

		// Set the width and height of the ball based on the size of the field.
		Height = (int)(field.Height * Scale / 20);
		Width = Height;

		// Move the ball to the center of the field.
		SetCenter(field.Center);

		// The ball starts a neutral color.
		Color = Palette.Neutral;
	}

	public void Initialize()
	{
		// Set everything to starting positions using Reset.
		Reset();
		Initialized = true;
	}

	public void Update(GameTime time)
	{
		// The ball's positions move according to velocity times the time that has passed.
		X += (int)(_xvelocity * time.ElapsedGameTime.TotalMilliseconds);
		Y += (int)(_yvelocity * time.ElapsedGameTime.TotalMilliseconds);

		// If we are moving to the right
		if(_xvelocity > 0)
		{
			// Increase our speed by a small amount
			_xvelocity += Acceleration / 1000;
			// If we hit the paddle, trigger a bounce, but don't notify the Field
			if(HitRightPaddle) Bounce(Wall.Right, false);
			// If we hit a wall, bounce and notify.
			else if(HitRightWall) Bounce(Wall.Right, true);
		}
		// Otherwise we are moving to the left
		else
		{
			_xvelocity -= Acceleration / 1000;
			if(HitLeftPaddle) Bounce(Wall.Left, false);
			else if(HitLeftWall) Bounce(Wall.Left, true);
		}
		// If we are moving down
		if(_yvelocity > 0)
		{
			_yvelocity += Acceleration / 1000;
			if(HitBottomWall) Bounce(Wall.Botton, false);
		}
		// Otherwise, we are moving up.
		else
		{
			_yvelocity -= Acceleration / 1000;
			if(HitTopWall) Bounce(Wall.Top, false);
		}
	}

	/// <summary>
	/// The logic performed when the ball hits something.
	/// </summary>
	/// <param name="wall">The wall (or paddle) that was hit.</param>
	/// <param name="raiseEvent">Whether the Field should be notified (for score purposes)</param>
	public void Bounce(Wall wall, bool raiseEvent = false)
	{
		// If we are notifying the field, that means the ball has hit the left or right wall.
		// Play the pong sound indicating the ball is out.
		if(raiseEvent)
		{
			var instance = pong.CreateInstance();
			instance.Volume = Volume / 10.0f;
			instance.Play();
		}
		// Otherwise, play the ping sound.
		else
		{
			// Have the volume slowly increase the faster the ball is going,
			// but don't exceed max volume (1)
			float volume = 0.1f * (float)(Math.Abs(_xvelocity) + Math.Abs(_yvelocity)) * Volume / 10.0f;
			if(volume > 1.0f) volume = 1.0f;
			var instance = ping.CreateInstance();
			instance.Volume = volume;
			instance.Play();
		}
		// randomly deviate when we bounce, don't bounce in a perfect geometric reflection
		double deviation = (rand.NextDouble() - 0.5) * Deviance;
		double newXvelocity = _xvelocity + _xvelocity * deviation;
		double newYVelocity = _yvelocity - _yvelocity * deviation;
		
		// Change the color of the ball and bounce differently depending on which wall we hit.
		switch(wall)
		{
			case Wall.Left:
				Color = Palette.LeftPaddle;
				_xvelocity = -_xvelocity;
				break;
			case Wall.Right:
				Color = Palette.RightPaddle;
				_xvelocity = -_xvelocity;
				break;
			case Wall.Top:
			case Wall.Botton:
				_yvelocity = -_yvelocity;
				break;
		}
		if(raiseEvent && BallBounced is not null) BallBounced(wall);
	}
	// We have hit the left wall if our left side has passed the field's left side.
	private bool HitLeftWall => Left < field.Left;
	// We have hit the right wall if our right side has passed the field's right side.
	private bool HitRightWall => Right > field.Right;
	// Likewise for top and bottom.
	private bool HitTopWall => Geometry.Top < field.Geometry.Top;
	private bool HitBottomWall => Geometry.Bottom > field.Geometry.Bottom;

	// We have hit the left paddle if our left has passed the paddle's right,
	// and our center is within the bounds of the paddle.
	private bool HitLeftPaddle =>
		Left < field.LeftPaddle.Right && Top < field.LeftPaddle.Bottom && Bottom > field.LeftPaddle.Top;
	// Likewise for the right paddle.
	private bool HitRightPaddle =>
		Right > field.RightPaddle.Left && Top < field.RightPaddle.Bottom && Bottom > field.RightPaddle.Top;



	private double _xvelocity;
	private double _yvelocity;
}
