using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
    class Background : IChunkItem
    {
        public string Name { get; private set; }
        private TexturePage texturePage;

        public void Load(BinaryReader br, UInt32 address)
        {
            br.Jump(address);

            Name = br.ReadStringFromOffset();
            br.Advance(12); // Unknown[3]
            UInt32 tpOff = br.ReadUInt32();
            texturePage = new TexturePage();
            texturePage.Load(br, tpOff);

            br.JumpBack();
        }

        public Bitmap GetBitmap()
        {
            return texturePage.GetPage();
        }
    }
}
