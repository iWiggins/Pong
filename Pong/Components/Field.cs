using GameFrame.Core.Geometrics;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using System;
using System.Collections.Generic;

namespace Pong.Components;
/// <summary>
/// A component containing the main pong animate gameplay: The paddles and ball.
/// </summary>
internal class Field : GeometricComponent, IReset
{
	public readonly Ball Ball;
	public readonly Paddle LeftPaddle;
	public readonly Paddle RightPaddle;

	/// <summary>
	/// The current score.
	/// </summary>
	public int Score { get; private set; } = 0;
	/// <summary>
	/// Handles when the score is changed.
	/// </summary>
	/// <param name="oldScore">The old score.</param>
	/// <param name="newScore">The new score.</param>

	public delegate void ScoreChangedHandler(int oldScore, int newScore);
	/// <summary>
	/// Raised when the score is changed.
	/// </summary>
	public event ScoreChangedHandler? ScoreChanged;

	/// <param name="keyboard">A reference to the keyboard.</param>
	/// <param name="mouse">A reference to the mouse.</param>
	/// <param name="ball">A texture for the ball.</param>
	/// <param name="paddle">A texture for the paddle.</param>
	/// <param name="ping">Sound effect for the ball bouncing.</param>
	/// <param name="pong">Sound effect for the ball going out.</param>
	/// <param name="settings">The game settings.</param>
	/// <param name="rand">A random number generator.</param>
	public Field(Keyboard keyboard, Mouse mouse, Texture2D ball, Texture2D paddle, SoundEffect ping, SoundEffect pong, PongSettings settings, Random rand)
	{
		//First, create a ball with sound effect volume set by game settings.
		Ball = new(this, ball, ping, pong, rand)
		{
			Volume = settings.Volume
		};
		// Add our event handling function for when the ball bounces.
		Ball.BallBounced += CheckCollision;
		_children.Add(Ball);

		// Next, the left paddle.
		LeftPaddle = new(this, paddle, mouse)
		{
			Side = Wall.Left,
			Color = Palette.LeftPaddle
		};
		// Give the left paddle controls based on settings.
		AddControls(LeftPaddle, keyboard, settings.LeftPaddle);
		_children.Add(LeftPaddle);
		
		// Finally, right paddle.
		RightPaddle = new(this, paddle, mouse)
		{
			Side = Wall.Right,
			Color = Palette.RightPaddle
		};
		// With controls based on settings.
		AddControls(RightPaddle, keyboard, settings.RightPaddle);
		_children.Add(RightPaddle);		
	}

	public void Reset()
	{
		//Just set the score back to 0 when the game resets.
		// No need to reset the ball and paddles,
		// the Frame will call their reset functions automatically.
		Score = 0;
	}
	/// <summary>
	/// Refreshes the round when the ball goes out of bounds.
	/// </summary>
	public void Refresh()
	{
		Ball.Reset();
	}
	/// <summary>
	/// Adjusts the game state when the ball goes out of bounds.
	/// </summary>
	/// <param name="wall"></param>
	private void CheckCollision(Wall wall)
	{
		// remember the old score to use in the event later.
		int oldScore = Score;
		// If the ball has not reached max starting speed, increase it by 20%.
		if(Ball.Speed < 3) Ball.Speed *= 1.2;

		// Change the score based on which wall was hit. Refresh only if the left or right walls were hit.
		switch(wall)
		{
			case Wall.Left:
				Score -= 1;
				Refresh();
				break;
			case Wall.Right:
				Score += 1;
				Refresh();
				break;
			default:
				break;
		}
		// If the score changed, raise the event.
		if(ScoreChanged is not null && oldScore != Score) ScoreChanged(oldScore, Score);
	}

	public override IEnumerable<IComponent> Children => _children;

	public override bool HasChildren => true;

	public override bool AddChild(IComponent component) => false;
	public override void Invalidate() { }
	public override bool RemoveChild(IComponent component) => false;

	/// <summary>
	/// Sets up a paddle's controls based on settings.
	/// </summary>
	/// <param name="paddle">The paddle to setup.</param>
	/// <param name="keyboard">A reference to the keyboard.</param>
	/// <param name="controller">What kind of controller this paddle has.</param>
	private void AddControls(Paddle paddle, Keyboard keyboard, Controller controller)
	{
		switch(controller)
		{
			case Controller.WASD:
				AddWASD(paddle, keyboard);
				break;
			case Controller.Arrows:
				AddArrows(paddle, keyboard);
				break;
			case Controller.Mouse:
				paddle.MouseControl = true;
				break;
			case Controller.CPU:
			default:
				AddCPU(paddle);
				break;
		}
	}
	/// <summary>
	/// Sets up the events for a paddle to be controlled by the W and S keys.
	/// </summary>
	private void AddWASD(Paddle paddle, Keyboard keyboard)
	{
		keyboard.KeyW.KeyPressed += k => paddle.MoveUp();
		keyboard.KeyW.KeyReleased += (k, d) => paddle.Stop();
		keyboard.KeyS.KeyPressed += k => paddle.MoveDown();
		keyboard.KeyS.KeyReleased += (k, d) => paddle.Stop();
	}
	/// <summary>
	/// Sets up events for a paddle to be controlled by the up and down arrow keys.
	/// </summary>
	private void AddArrows(Paddle paddle, Keyboard keyboard)
	{
		keyboard.KeyUp.KeyPressed += k => paddle.MoveUp();
		keyboard.KeyUp.KeyReleased += (k, d) => paddle.Stop();
		keyboard.KeyDown.KeyPressed += k => paddle.MoveDown();
		keyboard.KeyDown.KeyReleased += (k, d) => paddle.Stop();
	}
	/// <summary>
	/// Creates a PongAI to control this paddle.
	/// </summary>
	private void AddCPU(Paddle paddle) => // difficulty is set at 1.5. The fastest speed the paddle can move is 1.5.
		_children.Add(new PongAI(Ball, paddle, this, 1.5));

	private readonly List<IComponent> _children = [];
}
