using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MkscEdit.Compression;
using MkscEdit.Types;
using System.Diagnostics;

namespace MkscEdit.Extract
{
    public class Rom
    {
        public Tile[][] tiles;

        public Rom(){
            tiles = new Tile[0][];
        }
        public unsafe bool OpenRom(string path)
        {
            var t = File.ReadAllBytes(path);
            if (!VerifyRom(t))
            {
                return false;
            }
            Program.file = t;
            return true;
        }
        public unsafe byte[] DecompressRange(byte[] file, int startPos)
        {
            byte[] compData = new byte[4096];
            fixed (byte* output = compData)
            {
                fixed (byte* rom = &file[startPos])
                {
                    if(!LZ77.Decompress(rom, output)) throw new Exception("Not decompressable");
                }
            }
            return compData;
        }

        public bool VerifyRom(byte[] file)
        {
            //Check rom code   A                      M                      K                      E
            if ((file[0xac] == 0x41) & (file[0xad] == 0x4d) & (file[0xae] == 0x4b) & (file[0xaf] == 0x45))
            {
                return true;
            }
            return false;

        }

        public void ExtractTileGraphics()
        {
            tiles = new Tile[Enum.GetValues(typeof(Track)).Length][];
            for (int t = 0; t < Enum.GetValues(typeof(Track)).Length; t++)
            {
                int[] track = Program.offsets[t];
                int t1 = track[(int)TrackOffset.Tiles1];
                int t2 = track[(int)TrackOffset.Tiles2];
                int t3 = track[(int)TrackOffset.Tiles3];
                int t4 = track[(int)TrackOffset.Tiles4];
                int pal = track[(int)TrackOffset.Palette];
                byte[] tilegfx = new byte[4096 * 4];
                Array.Copy(DecompressRange(Program.file, t1), 0, tilegfx, 4096 * 0, 4096);
                Array.Copy(DecompressRange(Program.file, t2), 0, tilegfx, 4096 * 1, 4096);
                Array.Copy(DecompressRange(Program.file, t3), 0, tilegfx, 4096 * 2, 4096);
                Array.Copy(DecompressRange(Program.file, t4), 0, tilegfx, 4096 * 3, 4096);
                byte[] rawpal = new byte[128];
                Array.Copy(Program.file, pal, rawpal, 0, 128);
                Palette palette = new Palette(rawpal);
                Tile[] tilearr = Tile.GenerateTiles(tilegfx, palette);
                tiles[t] = tilearr;
            }
        }
    }
}