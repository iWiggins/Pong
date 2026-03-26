using GameFrame.Core.Geometrics;
using GameFrame.Core.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Pong.Components;
internal class Field : GeometricComponent
{
	public readonly Ball Ball;

	public Field(Texture2D ball, Random rand)
	{
		Ball = new(this, ball, rand);
	}

	public override IEnumerable<IComponent> Children => [Ball];

	public override bool HasChildren => true;

	public override bool AddChild(IComponent component) => false;
	public override void Invalidate() { }
	public override bool RemoveChild(IComponent component) => false;
}
