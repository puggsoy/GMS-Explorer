using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
	class BGND : ListChunk<Background>
	{
		public static void Load(BinaryReader br)
		{
			Instance = null;

			long chunkOffset = br.FindChunk("BGND");
			br.Jump(chunkOffset);

			Instance = new BGND();
			Instance.LoadData(br);

			br.JumpBack();
		}

		public static BGND Instance { get; private set; }

		private BGND() { }

		public Background GetBackground(int index)
		{
			return m_contents[index];
		}
	}
}
