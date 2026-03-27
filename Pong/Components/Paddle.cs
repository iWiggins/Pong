using GameFrame.Components;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Components;
internal class Paddle(Field field, Texture2D texture): Image(texture), IInitialize, IReset, IUpdate
{
	public required Wall Side { get; init; }

	public bool Initialized { get; private set; } = false;

	public double Speed { get; set; } = 2;
	public double Scale { get; set; } = 1;

	public void MoveUp()
	{
		_velocity = -Speed;
	}

	public void MoveDown()
	{
		_velocity = Speed;
	}

	public void Stop()
	{
		_velocity = 0;
	}

	public void Initialize()
	{
		Height = (int)(field.Height * Scale / 8);
		Width = Height / 4;
		Reset();
		Initialized = true;
	}

	public void Reset()
	{
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
		Y += (int)(_velocity * time.ElapsedGameTime.TotalMilliseconds);
		if(Top < field.Top) Top = field.Top;
		else if(Bottom > field.Bottom) Bottom = field.Bottom;
	}

	private double _velocity = 0;
}
