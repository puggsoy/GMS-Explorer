using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace GMS_Explorer
{
	class TXTR : ListChunk<Spritesheet>
	{
		public static void Load(BinaryReader br)
		{
			Instance = null;

			long chunkOffset = br.FindChunk("TXTR");
			br.Jump(chunkOffset);

			Instance = new TXTR();
			Instance.LoadData(br);

			br.JumpBack();
		}

		public static TXTR Instance { get; private set; }

		private TXTR() { }

		public Bitmap GetBitmap(int index)
		{
			return m_contents[index].Image;
		}
	}
}
