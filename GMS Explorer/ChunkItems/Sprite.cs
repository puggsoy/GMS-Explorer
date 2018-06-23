using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
	class Sprite : IChunkItem
	{
		public string Name { get; private set; }
		private UInt32 width;
		private UInt32 height;
		private Rectangle box;
		private UInt32 boundingBoxMode;
		private UInt32 sepMasks;
		private UInt32 originX;
		private UInt32 originY;
		public TexturePage[] texturePages;

		public int FrameNum { get { return texturePages.Length; } }

		public void Load(BinaryReader br, UInt32 address)
		{
			br.Jump(address);

			Name = br.ReadStringFromOffset();
			width = br.ReadUInt32();
			height = br.ReadUInt32();
			box = new Rectangle();
			box.X = (int)br.ReadUInt32(); // Left
			box.Width = (int)br.ReadUInt32() - box.X; // Right
			UInt32 bottom = br.ReadUInt32();
			box.Y = (int)br.ReadUInt32(); // Top
			box.Height = (int)bottom - box.Y; // Bottom
			br.Advance(12); // Unknown[4]
			boundingBoxMode = br.ReadUInt32();
			sepMasks = br.ReadUInt32();
			originX = br.ReadUInt32();
			originY = br.ReadUInt32();

			UInt32 textureCount = br.ReadUInt32();
			texturePages = new TexturePage[textureCount];

			for (int i = 0; i < texturePages.Length; i++)
			{
				UInt32 offset = br.ReadUInt32();
				TexturePage tp = new TexturePage();
				tp.Load(br, offset);

				texturePages[i] = tp;
			}

			br.JumpBack();
		}

		public Bitmap[] GetFrames()
		{
			Bitmap[] bitmaps = new Bitmap[texturePages.Length];

			for (int i = 0; i < texturePages.Length; i++)
			{
				Bitmap bmp = texturePages[i].GetPage();

				bitmaps[i] = bmp;
			}

			return bitmaps;
		}

		public Bitmap GetFrame(int index)
		{
			return texturePages[index].GetPage();
		}
	}
}
