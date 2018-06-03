using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
    class SPRT : ListChunk<Sprite>
    {
        public static void Load(BinaryReader br)
        {
            Instance = null;

            long chunkOffset = br.FindChunk("SPRT");
            br.Jump(chunkOffset);

            Instance = new SPRT();
            Instance.LoadData(br);

            br.JumpBack();
        }

        public static SPRT Instance { get; private set; }

        private SPRT() {}

        public Sprite GetSprite(int index)
        {
            return m_contents[index];
        }
    }
}
