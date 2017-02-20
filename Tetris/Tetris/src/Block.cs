using System;

namespace Tetris
{
	/// <summary>
	/// Block.
	/// </summary>
	public class Block
	{
		public BlockColor Color { get; set; }

		public BlockColor BackgroundColor { get; set; }

		public BlockColor DisplayedColor{ get { return Color ?? BackgroundColor ?? null; } }

		public bool Occupied{ get { return Color != null; } }

		public Block (BlockColor color, BlockColor background)
		{
			this.Color = color;
			this.BackgroundColor = background;
		}

		public Block() : this(null,null){}

		public void Clear()
		{
			Color = null;
			BackgroundColor = null;
		}
	}
}