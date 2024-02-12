using MkscEdit.Types;
using MkscEdit.Compression;

namespace MkscEdit.Extract
{
    public class Track
    {
        public byte[] TrackData;
        public int Address;
        public int TilesetPointerTable;
        public int LayoutPointerTable;
        public int PalettePointer;
        public int TileBehaviours;
        public int ObjectsPointer;
        public int OverlayPointer;
        public int ItemBoxesPointer;
        public int EndlinePointer;
        public int[] TileBlocks;
        public int[] LayoutBlocks;

        public Tile[] Tiles;
        public Palette palette;
        
        public Track(int address,int nextTrackAddr)
        {
            TrackData = new byte[nextTrackAddr - address];
            Array.Copy(Program.file, address, TrackData, 0, nextTrackAddr - address);
            Address = address;

            TilesetPointerTable = LittleEndianInt(TrackData[0x80..0x84]);
            LayoutPointerTable = 0x100;
            PalettePointer = LittleEndianInt(TrackData[0x84..0x88]);
            TileBehaviours = LittleEndianInt(TrackData[0x88..0x8c]);
            ObjectsPointer = LittleEndianInt(TrackData[0x8c..0x90]);
            OverlayPointer = LittleEndianInt(TrackData[0x90..0x94]);
            ItemBoxesPointer = LittleEndianInt(TrackData[0x94..0x98]);
            EndlinePointer = LittleEndianInt(TrackData[0x98..0x9c]);

            TileBlocks = [
                TilesetPointerTable + LittleEndianShort(TrackData[(TilesetPointerTable    )..(TilesetPointerTable + 2)]),
                TilesetPointerTable + LittleEndianShort(TrackData[(TilesetPointerTable + 2)..(TilesetPointerTable + 4)]),
                TilesetPointerTable + LittleEndianShort(TrackData[(TilesetPointerTable + 4)..(TilesetPointerTable + 6)]),
                TilesetPointerTable + LittleEndianShort(TrackData[(TilesetPointerTable + 6)..(TilesetPointerTable + 8)]),
            ];
            LayoutBlocks = [
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 0)..(LayoutPointerTable + 2)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 2)..(LayoutPointerTable + 4)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 4)..(LayoutPointerTable + 6)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 6)..(LayoutPointerTable + 8)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 8)..(LayoutPointerTable + 10)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 10)..(LayoutPointerTable + 12)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 12)..(LayoutPointerTable + 14)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 14)..(LayoutPointerTable + 16)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 16)..(LayoutPointerTable + 18)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 18)..(LayoutPointerTable + 20)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 20)..(LayoutPointerTable + 22)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 22)..(LayoutPointerTable + 24)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 24)..(LayoutPointerTable + 26)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 26)..(LayoutPointerTable + 28)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 28)..(LayoutPointerTable + 30)]),
                LayoutPointerTable + LittleEndianShort(TrackData[(LayoutPointerTable + 30)..(LayoutPointerTable + 32)]),
            ];
            int t1 = TileBlocks[0];
            int t2 = TileBlocks[1];
            int t3 = TileBlocks[2];
            int t4 = TileBlocks[3];
            int pal = PalettePointer;
            byte[] tilegfx = new byte[4096 * 4];
            Array.Copy(LZ77.DecompressRange(TrackData, t1), 0, tilegfx, 4096 * 0, 4096);
            Array.Copy(LZ77.DecompressRange(TrackData, t2), 0, tilegfx, 4096 * 1, 4096);
            Array.Copy(LZ77.DecompressRange(TrackData, t3), 0, tilegfx, 4096 * 2, 4096);
            Array.Copy(LZ77.DecompressRange(TrackData, t4), 0, tilegfx, 4096 * 3, 4096);
            byte[] rawpal = new byte[128];
            Array.Copy(TrackData, pal, rawpal, 0, 128);
            Palette palette = new Palette(rawpal);
            Tiles = Tile.GenerateTiles(tilegfx, palette);
        }
        public void PackData()
        {
            byte[] testReversal = Tile.GetTileBytes(Tiles);
            byte[] t1 = LZ77.CompressRange(testReversal[(4096*0)..(4096*1)],0,4096);
            byte[] t2 = testReversal[(4096 * 1)..(4096 * 2)];
            byte[] t3 = testReversal[(4096 * 2)..(4096 * 3)];
            byte[] t4 = testReversal[(4096 * 3)..(4096 * 4)];
        }
        public static int LittleEndianInt(byte[] a)
        {
            return (a[3] << 24 | a[2] << 16 | a[1] << 8 | a[0]);
        }
        public static int LittleEndianShort(byte[] a)
        {
            return ( a[1] << 8 | a[0]);
        }
    }
    public enum TrackId
    {
        SNESMarioCircuit1,
        SNESDonutPlains1,
        SNESGhostValley1,
        SNESBowserCastle1,
        //SNESMarioCircuit2,
        SNESChocoIsland1,
        //SNESGhostValley2,
        //SNESDonutPlains2,
        //SNESBowserCastle2,
        //SNESMarioCircuit3,
        SNESKoopaBeach1,
        //SNESChocoIsland2,
        SNESVanillaLake1,
        //SNESBowserCastle3,
        //SNESMarioCircuit4,
        //SNESDonutPlains3,
        //SNESKoopaBeach2,
        //SNESGhostValley3,
        //SNESVanillaLake2,
        //SNESRainbowRoad,
        //SNESBattleCourse1,
        //SNESBattleCourse2,
        //SNESBattleCourse3,
        //SNESBattleCourse4,
        PeachCircuit,
        ShyGuyBeach,
        SunsetWilds,
        BowserCastle1,
        LuigiCircuit,
        RiversidePark,
        YoshiDesert,
        //BowserCastle2,
        MarioCircuit,
        //CheepCheepIsland,
        RibbonRoad,
        //BowserCastle3,
        SnowLand,
        BooLake,
        CheeseLand,
        RainbowRoad,
        SkyGarden,
        //BrokenPier,
        //BowserCastle4,
        //LakesidePark,
        //BattleCourse1,
        //BattleCourse2,
        //BattleCourse3,
        //BattleCourse4,
        //TestTrack,
    }
    public class Offsets
    {
        public Offsets()
        {
            // commented tracks need to be fixed
            Program.tracks = [
                new Track(0x2580d4, 0x25b17c),
                new Track(0x25b17c, 0x25e380),
                new Track(0x25e380, 0x260354),
                new Track(0x260354, 0x262c1c),
                //new TrackOffset(Track.SNESMarioCircuit2,0x262c1c, 0x00000fa4, 0x00000fa4, 0x00001024, 0x00001618, 0x00001600, 0x000016e8, 0x000016fc),
                new Track(0x264348, 0x266de8),
                //new TrackOffset(Track.SNESGhostValley2, 0x266de8, 0x00000c58, 0x00000c58, 0x00000cd8, 0x00001434, 0x00001400, 0x00001504, 0x00001520),
                //new TrackOffset(Track.SNESDonutPlains2, 0x268338, 0x00001558, 0x00001558, 0x000015d8, 0x00001c14, 0x00001c00, 0x00001ce4, 0x00001cf8),
                //new TrackOffset(Track.SNESBowserCastle2,0x26a060, 0x00000fb8, 0x00000fb8, 0x00001038, 0x00001890, 0x00001800, 0x00001960, 0x00001974),
                //new TrackOffset(Track.SNESMarioCircuit3,0x26ba04, 0x000010fc, 0x000010fc, 0x0000117c, 0x00001824, 0x00001800, 0x000018f4, 0x00001908),
                new Track(0x26d33c, 0x271634),
                //new TrackOffset(Track.SNESChocoIsland2, 0x26fd90, 0x0000123c, 0x0000123c, 0x000012bc, 0x00001790, 0x00001700, 0x00001860, 0x00001874),
                new Track(0x271634, 0x273c68),
                //new TrackOffset(Track.SNESBowserCastle3,0x273c68, 0x00001030, 0x00001030, 0x000010b0, 0x0000177c, 0x00001700, 0x0000184c, 0x00001860),
                //new TrackOffset(Track.SNESMarioCircuit4,0x2754f8, 0x00001114, 0x00001114, 0x00001194, 0x00001860, 0x00001800, 0x00001930, 0x00001944),
                //new TrackOffset(Track.SNESDonutPlains3, 0x276e6c, 0x000015ec, 0x000015ec, 0x0000166c, 0x00001cf0, 0x00001c00, 0x00001dc0, 0x00001dd4),
                //new TrackOffset(Track.SNESKoopaBeach2,  0x278c70, 0x000014a4, 0x000014a4, 0x00001524, 0x00001944, 0x00001900, 0x00001a14, 0x00001a24),
                //new TrackOffset(Track.SNESGhostValley3, 0x27a6c4, 0x00000cc0, 0x00000cc0, 0x00000d40, 0x00001508, 0x00001500, 0x000015d8, 0x000015ec),
                //new TrackOffset(Track.SNESVanillaLake2, 0x27bce0, 0x000011f4, 0x000011f4, 0x00001274, 0x00001790, 0x00001700, 0x00001860, 0x00001874),
                // throws other error new TrackOffset(Track.SNESRainbowRoad,  0x27d584, 0x00000c88, 0x000016e4, 0x00001764, 0x00001e78, 0x00001e00, 0x00001f48, 0x00001f5c),
                //new TrackOffset(Track.SNESBattleCourse1,0x27f510, 0x00000a8c, 0x00000a8c, 0x00000c8c, 0x00000000, 0x00000000, 0x00000000, 0x00000000),
                //new TrackOffset(Track.SNESBattleCourse2,0x280580, 0x00000ac0, 0x00000ac0, 0x00000cc0, 0x00000000, 0x00000000, 0x00000000, 0x00000000),
                //new TrackOffset(Track.SNESBattleCourse3,0x281624, 0x00000a40, 0x00000a40, 0x00000c40, 0x00000000, 0x00000000, 0x00000000, 0x00000000),
                //new TrackOffset(Track.SNESBattleCourse4,0x282c24, 0x00000afc, 0x00000afc, 0x00000cfc, 0x00000000, 0x00000000, 0x00000000, 0x00000000),
                new Track(0x283d04, 0x28a020),
                new Track(0x28a020, 0x29044c),
                new Track(0x29044c, 0x29aadc),
                new Track(0x29aadc, 0x29fc74),
                new Track(0x29fc74, 0x2a6be8),
                new Track(0x2a6be8, 0x2ae488),
                new Track(0x2ae488, 0x2b58e8),
                //new TrackOffset(Track.BowserCastle2,    0x2b58e8, 0x000029e4, 0x000029e4, 0x00002a64, 0x00003580, 0x00003500, 0x00003674, 0x00003690),
                new Track(0x2b8fa8, 0x2bf16c),
                //new TrackOffset(Track.CheepCheepIsland, 0x2bf16c, 0x00003f14, 0x00003f14, 0x00003f94, 0x00005b64, 0x00005b00, 0x00005c78, 0x00005ca0),
                new Track(0x2c4e3c, 0x2cb3f4),
                //new TrackOffset(Track.BowserCastle3,    0x2cb3f4, 0x00002a60, 0x00002a60, 0x00002ae0, 0x000037ac, 0x00003700, 0x000038c0, 0x000038d8),
                new Track(0x2cecfc, 0x2d5f40),
                new Track(0x2d5f40, 0x2dbab0),
                new Track(0x2dbab0, 0x2e2940),
                new Track(0x2e2940, 0x2e7d08),
                new Track(0x2e7d08, 0x2ee838),
                //new TrackOffset(Track.BrokenPier,       0x2ee838, 0x00002e64, 0x00002e64, 0x00002ee4, 0x00003df4, 0x00003e00, 0x00003f24, 0x00003f4c),
                //new TrackOffset(Track.BowserCastle4,    0x2f27b4, 0x00003260, 0x00003260, 0x000032e0, 0x000040f0, 0x00004100, 0x00004214, 0x0000423c),
                //new TrackOffset(Track.LakesidePark,     0x2f6a20, 0x00004b80, 0x00004b80, 0x00004c00, 0x00006470, 0x00006400, 0x000065a4, 0x000065c4),
                //new TrackOffset(Track.BattleCourse1,    0x2fd014, 0x00000c2c, 0x00000c2c, 0x00000cac, 0x000011c8, 0x00000000, 0x000011cc, 0x000011f4),
                //new TrackOffset(Track.BattleCourse2,    0x2fe234, 0x00000c80, 0x00000c80, 0x00000d00, 0x000010fc, 0x00000000, 0x00001100, 0x00001118),
                //new TrackOffset(Track.BattleCourse3,    0x2ff378, 0x00001124, 0x00001124, 0x000011a4, 0x0000157c, 0x00000000, 0x00001580, 0x000015a4),
                //new TrackOffset(Track.BattleCourse4,    0x300948, 0x00000b9c, 0x00000b9c, 0x00000c1c, 0x00000e68, 0x00000000, 0x00000e6c, 0x00000e84),
                //new TrackOffset(Track.TestTrack,        0x3017f8, 0x000010fc, 0x000010fc, 0x0000117c, 0x00001644, 0x00001600, 0x0000164c, 0x00001650),
            ];
        }
    }
}