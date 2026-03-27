using GameFrame.Core;
using GameFrame.Core.Interfaces;
using GameFrame.Core.Management;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Frames;

namespace Pong;
internal class PongGame : ManagedGame
{
	protected override Frame CreateFirstFrame(ContentManager content, SpriteBatch sprites, IBoundsProvider bounds) =>
		new MainMenu(new(content), sprites, bounds);
}
