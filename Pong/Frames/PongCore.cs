using GameFrame.Core;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Components;
using System;

namespace Pong.Frames;
internal class PongCore: Frame
{
	public PongCore(ContentManager content, SpriteBatch spriteBatch, IBoundsProvider bounds) : base(spriteBatch, bounds)
	{
		_rand = new Random();
		_gameField = new(content.Load<Texture2D>("ball"), _rand)
		{
			X = bounds.Bounds.X,
			Y = bounds.Bounds.Y,
			Width = bounds.Bounds.Width,
			Height = bounds.Bounds.Height
		};
	}
	protected override void PreInitialize()
	{
		Keyboard.KeyEscape.KeyPressed += OnEscapeKey;
		Keyboard.KeySpace.KeyReleased += OnSpacebar;
		Root.AddChild(_gameField);
	}

	private void OnSpacebar(Microsoft.Xna.Framework.Input.Keys key, double duration)
	{
		_gameField.Ball.Reset();
	}
	private void OnEscapeKey(Microsoft.Xna.Framework.Input.Keys key)
	{
		Exit(null);
	}

	private readonly Field _gameField;
	private readonly Random _rand;

	
}
