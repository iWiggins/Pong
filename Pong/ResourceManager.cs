using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Pong;
internal class ResourceManager(ContentManager content)
{
	public Texture2D TextureBackground => content.Load<Texture2D>("textures/background");
	public Texture2D TextureBall => content.Load<Texture2D>("textures/ball");
	public Texture2D TextureButton => content.Load<Texture2D>("textures/button");
	public Texture2D TexturePaddle => content.Load<Texture2D>("textures/paddle");

	public SpriteFont FontTitle => content.Load<SpriteFont>("fonts/title");
	public SpriteFont FontButton => content.Load<SpriteFont>("fonts/button");
	public SpriteFont FontScore => content.Load<SpriteFont>("fonts/score");

	public Song SongMenu => content.Load<Song>("music/menu");
	public Song SongGame => content.Load<Song>("music/game");

	public SoundEffect SfxPing => content.Load<SoundEffect>("sfx/ping");
	public SoundEffect SfxPong => content.Load<SoundEffect>("sfx/pong");
}
