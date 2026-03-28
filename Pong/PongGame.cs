using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Core.Management;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using Pong.Frames;

namespace Pong;
/// <summary>
/// The game class for Pong.
/// Initializes default settings and creates a <see cref="MainMenu"/>.
/// </summary>
internal class PongGame : ManagedGame
{
	protected override Frame CreateFirstFrame(ContentManager content, SpriteBatch sprites, IBoundsProvider bounds)
	{
		// Create some default settings.
		PongSettings settings = new()
		{
			LeftPaddle = Controller.WASD,
			RightPaddle = Controller.CPU,
			Volume = 5
		};

		// Create the main menu.
		// The first parameter creates a ResourceManager wrapping the ContentManager.
		return new MainMenu(new(content), sprites, bounds, settings);
	}
}
