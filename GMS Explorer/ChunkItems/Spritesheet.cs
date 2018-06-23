using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
	class Spritesheet : IChunkItem
	{
		public Bitmap Image { get; private set; }

		public void Load(BinaryReader br, UInt32 address)
		{
			br.Jump(address);

			br.ReadInt32(); // unknown, seems to always be 0
			UInt32 pngOffset = br.ReadUInt32();

			br.Jump(pngOffset);
			Int32 pngLength = br.GetPNGLength(pngOffset);
			byte[] pngBytes = br.ReadBytes(pngLength);
			using (MemoryStream ms = new MemoryStream(pngBytes))
			{
				Image = new Bitmap(ms);
			}
			br.JumpBack();

			br.JumpBack();
		}
	}
}
