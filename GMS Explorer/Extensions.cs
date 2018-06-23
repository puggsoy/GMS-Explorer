using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GMS_Explorer
{
	static class Extensions
	{
		// String reading

		public static string ReadString(this BinaryReader br, uint count)
		{
			return new string(br.ReadChars((int)count));
		}

		public static string ReadStringFromOffset(this BinaryReader br)
		{
			UInt32 offset = br.ReadUInt32();

			br.Jump(offset - 4);
			UInt32 len = br.ReadUInt32();
			string ret = br.ReadString(len);
			br.JumpBack();

			return ret;
		}

		// Jumping

		private static Stack<long> jumpStack = new Stack<long>();

		public static void Jump(this BinaryReader br, long offset)
		{
			jumpStack.Push(br.BaseStream.Position);
			br.BaseStream.Seek(offset, SeekOrigin.Begin);
		}

		public static void JumpBack(this BinaryReader br)
		{
			if (jumpStack.Count <= 0)
				throw new Exception("No jump back address!");

			br.BaseStream.Seek(jumpStack.Pop(), SeekOrigin.Begin);
		}

		public static void Advance(this BinaryReader br, long offset)
		{
			br.BaseStream.Seek(offset, SeekOrigin.Current);
		}

		// Extra utility

		public static long FindChunk(this BinaryReader br, string chunkNm)
		{
			br.Jump(0);

			string nm;
			do
			{
				nm = br.ReadString(4);
				UInt32 len = br.ReadUInt32();

				if (nm == "FORM") continue; // FORM chunk holds all other chunks, so we don't want to skip it
				if (nm == chunkNm) break;

				br.BaseStream.Seek(len, SeekOrigin.Current);
			}
			while (br.BaseStream.Position < br.BaseStream.Length);

			long ret = br.BaseStream.Position - 8;

			br.JumpBack();
			return ret;
		}

		public static int GetPNGLength(this BinaryReader br, long offset)
		{
			br.Jump(offset);

			long magic = br.ReadInt64();

			if (magic != 0x0A1A0A0D474E5089) // Little endian
				throw new Exception("Invalid PNG header!");

			string chunk;

			do
			{
				byte[] lenBytes = br.ReadBytes(4);
				if (BitConverter.IsLittleEndian)
					Array.Reverse(lenBytes);
				int chunkLen = BitConverter.ToInt32(lenBytes, 0);
				chunk = br.ReadString(4);
				br.BaseStream.Seek(chunkLen + 4, SeekOrigin.Current); // + 4 to account for CRC
			}
			while (chunk != "IEND");

			int ret = (int)(br.BaseStream.Position - offset);

			br.JumpBack();
			return ret;
		}
	}
}
