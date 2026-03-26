using GameFrame.Core;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Frames;

namespace Pong;
internal class PongGame : GameFrame.Core.Management.ManagedGame
{
	protected override Frame CreateFirstFrame(ContentManager content, SpriteBatch sprites, IBoundsProvider bounds) => new PongCore(content, sprites, bounds);
}
