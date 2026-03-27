using GameFrame.Components;
using GameFrame.Components.Buttons;
using GameFrame.Components.Text;
using GameFrame.Core;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Frames;
internal class MainMenu(ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds) : Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		Image background = new(resources.TextureBackground, null)
		{
			Layer = -1,
			Geometry = Screen.Bounds
		};
		Root.AddChild(background);

		FlowLayout mainLayout = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(mainLayout);
		{
			mainLayout.AddFiller();

			FlowLayout topLayout = new(FlowLayout.Direction.Left);
			mainLayout.AddChild(topLayout, 4);
			{
				topLayout.AddFiller(4);

				BoundText title = new(resources.FontTitle)
				{
					Contents = "Pong",
					Color = Color.White
				};
				topLayout.AddChild(title, 4);
				topLayout.AddFiller(4);
			}

			mainLayout.AddFiller();

			FlowLayout buttonLayout = new(FlowLayout.Direction.Left);
			mainLayout.AddChild(buttonLayout, 2);
			{
				buttonLayout.AddFiller(2);

				HoverTextImageButton startButton = new(resources.FontButton, resources.TextureButton)
				{
					Contents = "Start",
					NormalColor = Color.Red,
					TextNormalColor = Color.Blue,
					HoverColor = Color.Blue,
					TextHoverColor = Color.Red
				};
				startButton.Released += (b, p, dt) => Exit(new PongCore(resources, SpriteBatch, Screen, this));
				buttonLayout.AddChild(startButton, 2);

				buttonLayout.AddFiller(2);

				HoverTextImageButton quitButton = new(resources.FontButton, resources.TextureButton)
				{
					Contents = "Quit",
					NormalColor = Color.Red,
					TextNormalColor = Color.Blue,
					HoverColor = Color.Blue,
					TextHoverColor = Color.Red
				};
				quitButton.Released += (b, p, dt) => Exit(null);
				buttonLayout.AddChild(quitButton, 2);

				buttonLayout.AddFiller(2);
			}

			mainLayout.AddFiller();
		}

		

	}
}
