using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
	class Chunk
	{
		protected string m_name;
		protected UInt32 length;

		public Chunk() {}

		protected virtual void LoadData(BinaryReader br)
		{
			m_name = br.ReadString(4);
			length = br.ReadUInt32();
		}
	}
}
