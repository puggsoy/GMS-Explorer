using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
    class TexturePage : IChunkItem
    {
        private Rectangle sourceRect; // Rectangle of page on sheet
        private UInt16 renderX; // Offsets within bounding box
        private UInt16 renderY;
        private Rectangle boundingBox;
        private UInt16 sheetID;

        public void Load(BinaryReader br, UInt32 address)
        {
            br.Jump(address);

            sourceRect = new Rectangle();
            sourceRect.X = br.ReadUInt16();
            sourceRect.Y = br.ReadUInt16();
            sourceRect.Width = br.ReadUInt16();
            sourceRect.Height = br.ReadUInt16();
            renderX = br.ReadUInt16();
            renderY = br.ReadUInt16();
            boundingBox = new Rectangle();
            boundingBox.X = br.ReadUInt16();
            boundingBox.Y = br.ReadUInt16();
            boundingBox.Width = br.ReadUInt16();
            boundingBox.Height = br.ReadUInt16();
            sheetID = br.ReadUInt16();

            br.JumpBack();
        }

        public Bitmap GetPage()
        {
            Bitmap source = TXTR.Instance.GetBitmap(sheetID);
            Bitmap dest = new Bitmap(boundingBox.Width, boundingBox.Height);
            Graphics g = Graphics.FromImage(dest);
            g.DrawImage(source, renderX, renderY, sourceRect, GraphicsUnit.Pixel);

            return dest;
        }
    }
}
