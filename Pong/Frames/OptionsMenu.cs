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
/// <summary>
/// The Pong options menu.
/// </summary>
/// <param name="cursor">The custom cursor we are using in the main menu.</param>
/// <param name="resources">The Pong resource manager</param>
/// <param name="spriteBatch">The game's spritebatch</param>
/// <param name="bounds">The boundry provider from the main menu</param>
/// <param name="settings">The pong settings file from the main menu</param>
/// <param name="parent">A reference to the main menu so we can return to it without creating a new object.</param>
internal class OptionsMenu(
	IMouseCursor? cursor,
	ResourceManager resources,
	SpriteBatch spriteBatch,
	IBoundsProvider bounds,
	PongSettings settings,
	Frame parent):
	Frame(spriteBatch, bounds)
{
	protected override void PreInitialize()
	{
		// Set our custom cursor in this frame.
		Mouse.Cursor = cursor;

		// Create a downward layout to organize the components.
		// Its geometry is the size of the screen.
		FlowLayout outerLayout = new(FlowLayout.Direction.Down)
		{
			Geometry = Screen.Bounds
		};
		// Add it to the root so it is actually connected.
		Root.AddChild(outerLayout);

		// Add some filler so there is space at the top of the screen.
		outerLayout.AddFiller();

		// Create a FillLayout to stretch the background and menu items
		// to fill the available space and stack on top of each other.
		FillLayout stack = new();
		outerLayout.AddChild(stack, 8);

		// Add filler so there is blank space on the bottom of the screen.
		outerLayout.AddFiller();

		// Create our background image and add it to the FillLayout
		Image background = new(resources.TextureBackground)
		{
			Layer = -1
		};
		stack.AddChild(background);

		// Create another downward flow for the menu items and buttons.
		FlowLayout mainLayout = new(FlowLayout.Direction.Down);
		stack.AddChild(mainLayout);

		//Create a horizontal flow for our three option selectors.
		FlowLayout midLayout = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(midLayout);

		// Filler so there is blank space on the left side of the menu.
		midLayout.AddFiller();

		// First, a selector for the left paddle's control.
		Multiselector leftSelector = new([
			new FramedImage(resources.TextureWASD),
			new FramedImage(resources.TextureArrows),
			new FramedImage(resources.TextureMouse),
			new FramedImage(resources.TextureCPU)
		])
		{
			Current = (int)settings.LeftPaddle
		};

		// Add an event handler so when the value is changed, we change the underlying setting.
		leftSelector.Changed += (oldValue, newValue) =>
		settings.LeftPaddle = (Controller)newValue;
		midLayout.AddChild(leftSelector);

		// Filler for space between the menu items.
		midLayout.AddFiller();

		// A numeric up down for the volume.
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
		// An event handler so when the value is changed, the setting changes.
		volume.ValueChanged += (oldValue, newValue) =>
		settings.Volume = newValue;
		midLayout.AddChild(volume);

		// Space between the menu items.
		midLayout.AddFiller();

		// A selector for the right paddle's control
		Multiselector rightSelector = new([
			new FramedImage(resources.TextureWASD),
			new FramedImage(resources.TextureArrows),
			new FramedImage(resources.TextureMouse),
			new FramedImage(resources.TextureCPU)
		])
		{
			Current = (int)settings.RightPaddle
		};
		// Event handler so the underlying setting changes
		rightSelector.Changed += (oldValue, newValue) =>
		settings.RightPaddle = (Controller)newValue;
		midLayout.AddChild(rightSelector);

		// Empty space on the right side of the menu items.
		midLayout.AddFiller();

		// Empty space between the menu items and the final row with the back button
		mainLayout.AddFiller();

		// A rightward flow to position the back button.
		FlowLayout buttonFlow = new(FlowLayout.Direction.Right);
		mainLayout.AddChild(buttonFlow);

		// Empty space to the left of the back button.
		buttonFlow.AddFiller(8);

		// Finally create the back button
		HoverTextImageButton backButton = new(resources.FontButton, resources.TextureButton)
		{
			Contents = "Back",
			NormalColor = Palette.LeftPaddle,
			TextNormalColor = Palette.RightPaddle,
			HoverColor = Palette.RightPaddle,
			TextHoverColor = Palette.LeftPaddle
		};
		// Create an event handler so we save the settings
		// and switch back to the main menu when back is pressed.
		backButton.Released += (b, p, dt) =>
		{
			settings.Save();
			Exit(parent);
		};
		buttonFlow.AddChild(backButton);

		// Blank space to the right of the back button.
		buttonFlow.AddFiller();

	}
}
