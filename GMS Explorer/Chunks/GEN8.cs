using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMS_Explorer
{
	class GEN8 : Chunk
	{
		public static void Load(BinaryReader br)
		{
			Instance = null;

			long chunkOffset = br.FindChunk("GEN8");
			br.Jump(chunkOffset);

			Instance = new GEN8();
			Instance.LoadData(br);

			br.JumpBack();
		}

		public static GEN8 Instance { get; private set; }

		public bool Debug { get; private set; }
		public Int32 Unknown1 { get; private set; } // Actually an Int24, but C# doesn't support that
		public string Filename { get; private set; }
		public UInt32 GameID { get; private set; }
		public string Name { get; private set; }
		public UInt32 MajorVersion { get; private set; }
		public UInt32 MinorVersion { get; private set; }
		public UInt32 ReleaseVersion { get; private set; }
		public UInt32 BuildVersion { get; private set; }
		public string DisplayName { get; private set; }
		public UInt32 SteamAppID { get; private set; }

		private GEN8() { }

		protected override void LoadData(BinaryReader br)
		{
			base.LoadData(br);

			byte debug = br.ReadByte();
			Debug = debug == 1;
			Unknown1 = (br.ReadUInt16() << 8) | br.ReadByte();
			Filename = br.ReadStringFromOffset();
			br.Advance(12); // ConfigOffset, LastObj, LastTile
			GameID = br.ReadUInt32();
			br.Advance(16); // Unknown2[4]
			Name = br.ReadStringFromOffset();
			MajorVersion = br.ReadUInt32();
			MinorVersion = br.ReadUInt32();
			ReleaseVersion = br.ReadUInt32();
			BuildVersion = br.ReadUInt32();
			br.Advance(40); // DefaultWindowWidth, DefaultWindowHeight, Info, LicenseMD5[0x10], LicenseCRC32, Timestamp
			DisplayName = br.ReadStringFromOffset();
			br.Advance(20); // ActiveTargets, Unknown3[4]
			SteamAppID = br.ReadUInt32();
		}
	}


}
