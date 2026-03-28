using GameFrame.Components;
using GameFrame.Components.Buttons;
using GameFrame.Components.Menu;
using GameFrame.Core;
using GameFrame.Core.Input;
using GameFrame.Core.Interfaces;
using GameFrame.Layout;
using Microsoft.Xna.Framework.Graphics;
using Pong.Data;

namespace Pong.Frames;
internal class OptionsMenu(IMouseCursor? cursor, ResourceManager resources, SpriteBatch spriteBatch, IBoundsProvider bounds, PongSettings settings, Frame parent):
	Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		Mouse.Cursor = cursor;
		FlowLayout outerLayout = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		Root.AddChild(outerLayout);

		outerLayout.AddFiller();

		FillLayout stack = new();
		outerLayout.AddChild(stack, 8);

		Image background = new(resources.TextureBackground)
		{
			Layer = -1
		};
		stack.AddChild(background);

		FlowLayout mainLayout = new(FlowLayout.Direction.Down);
		stack.AddChild(mainLayout);

		FlowLayout midLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(midLayout);

		midLayout.AddFiller();

		Multiselector leftSelector = new([
			new FramedImage(resources.TextureWASD),
			new FramedImage(resources.TextureArrows),
			new FramedImage(resources.TextureMouse),
			new FramedImage(resources.TextureCPU)
		])
		{
			Current = (int)settings.LeftPaddle
		};

		leftSelector.Changed += (oldValue, newValue) =>
		settings.LeftPaddle = (Controller)newValue;
		midLayout.AddChild(leftSelector);

		midLayout.AddFiller();

		NumericUpDown volume = new(
			resources.TextureUp,
			resources.TextureDown,
			resources.FontNumber,
			0,
			10,
			settings.Volume)
		{
			TextColor = Palette.Neutral
		};
		midLayout.AddChild(volume);

		midLayout.AddFiller();

		Multiselector rightSelector = new([
			new FramedImage(resources.TextureWASD),
			new FramedImage(resources.TextureArrows),
			new FramedImage(resources.TextureMouse),
			new FramedImage(resources.TextureCPU)
		])
		{
			Current = (int)settings.RightPaddle
		};

		rightSelector.Changed += (oldValue, newValue) =>
		settings.RightPaddle = (Controller)newValue;
		midLayout.AddChild(rightSelector);

		midLayout.AddFiller();

		mainLayout.AddFiller();

		FlowLayout buttonFlow = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(buttonFlow);

		buttonFlow.AddFiller(8);

		HoverTextImageButton backButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Back",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		backButton.Released += (b, p, dt) => Exit(parent);
		buttonFlow.AddChild(backButton);

		buttonFlow.AddFiller();

		outerLayout.AddFiller();
	}
}
