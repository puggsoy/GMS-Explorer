using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
    class ListChunk<T> : Chunk where T : IChunkItem, new()
    {
        protected List<T> m_contents;

        public List<T> Contents { get { return m_contents; } }

        public ListChunk() {}

        protected override void LoadData(BinaryReader br)
        {
            base.LoadData(br);

            m_contents = new List<T>();
            UInt32 addressCount = br.ReadUInt32();

            for (int i = 0; i < addressCount; i++)
            {
                UInt32 address = br.ReadUInt32();
                T item = new T();
                item.Load(br, address);

                m_contents.Add(item);
            }
        }
    }
}
