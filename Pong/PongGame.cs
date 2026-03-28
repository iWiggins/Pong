using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Core.Management;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;
using Pong.Frames;

namespace Pong;
internal class PongGame : ManagedGame
{
	protected override Frame CreateFirstFrame(ContentManager content, SpriteBatch sprites, IBoundsProvider bounds)
	{
		PongSettings settings = new()
		{
			LeftPaddle = Controller.WASD,
			RightPaddle = Controller.CPU,
			Volume = 5
		};
		return new MainMenu(new(content), sprites, bounds, settings);
	}
}
