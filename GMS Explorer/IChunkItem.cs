using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GMS_Explorer
{
    public interface IChunkItem
    {
        void Load(BinaryReader br, UInt32 address);
    }
}
