using GameFrame.Components;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using System;

namespace Pong.Components;
/// <summary>
/// Maintains the paddle's state and draws it.
/// </summary>
/// <param name="field">A reference to the field the paddle belongs to.</param>
/// <param name="texture">The paddle image's texture.</param>
/// <param name="mouse">A reference to the mouse.</param>
/// <remarks>
/// Paddle inherits Image because it needs to show up on screen,
/// and it doesn't have any further children.
/// </remarks>
internal class Paddle(Field field, Texture2D texture, Mouse mouse): Image(texture), IInitialize, IReset, IUpdate
{
	/// <summary>
	/// Which side is this paddle on, left or right.
	/// </summary>
	public required Wall Side { get; init; }

	public bool Initialized { get; private set; } = false;

	/// <summary>
	/// Whether the paddle is mouse controlled (instead of keyboard or cpu)
	/// </summary>
	public bool MouseControl { get; set; }

	/// <summary>
	/// How fast the paddle should move
	/// </summary>
	public double Speed { get; set; } = 2;
	/// <summary>
	/// How large the paddle is.
	/// </summary>
	public double Scale { get; set; } = 1;

	/// <summary>
	/// Starts moving the paddle up
	/// </summary>
	public void MoveUp()
	{
		_velocity = -Speed;
	}
	/// <summary>
	/// Starts moving the paddle down
	/// </summary>
	public void MoveDown()
	{
		_velocity = Speed;
	}
	/// <summary>
	/// Stops moving the paddle
	/// </summary>
	public void Stop()
	{
		_velocity = 0;
	}

	public void Initialize()
	{
		// Set the paddle dimensions based on the field size
		Height = (int)(field.Height * Scale / 8);
		Width = Height / 4;
		// Move the paddle to its initial position
		Reset();
		Initialized = true;
	}

	public void Reset()
	{
		// The paddle starts not moving, and at the midpoint of the screen on the side it is on.
		_velocity = 0;
		Y = field.Height / 2 + Height / 2;
		switch(Side)
		{
			case Wall.Left:
				Left = field.Left;
				break;
			case Wall.Right:
				Right = field.Right;
				break;
			default:
				throw new NotImplementedException();
		}
	}

	public void Update(GameTime time)
	{
		if(MouseControl)
		{
			// If we are using mouse controls, then don't use the movement functions.
			// Just set the paddle's Y to the mouse position.
			Y = mouse.Position.Y - Height / 2;

			// If the mouse would take us out of bounds, stop at the bounds.
			if(Top < field.Top) Top = field.Top;
			else if(Bottom > field.Bottom) Bottom = field.Bottom;
		}
		else
		{
			// Otherwise, move according to our fixed velocity time multiplied by the time passed.
			Y += (int)(_velocity * time.ElapsedGameTime.TotalMilliseconds);
			// If this would take us out of bounds, stop at the boundary.
			if(Top < field.Top) Top = field.Top;
			else if(Bottom > field.Bottom) Bottom = field.Bottom;
		}
	}

	private double _velocity = 0;
}
