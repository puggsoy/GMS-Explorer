using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
    class TPAG : ListChunk<TexturePage>
    {
        public static void Load(BinaryReader br)
        {
            Instance = null;

            long chunkOffset = br.FindChunk("TPAG");
            br.Jump(chunkOffset);

            Instance = new TPAG();
            Instance.LoadData(br);

            br.JumpBack();
        }

        static public TPAG Instance { get; private set; }

        private TPAG() {}

        public Bitmap GetPage(int index)
        {
            return m_contents[index].GetPage();
        }
    }
}
