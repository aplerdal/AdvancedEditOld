using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkscEdit.Extract
{
    // Tracks[Track.SMC1][Trackoffset.Tiles1]
    public enum Track
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
    public enum TrackOffset
    {
        Address,
        TrackLayoutPointer,
        TilesetPointerTable,
        Tiles1,
        Tiles2,
        Tiles3,
        Tiles4,
        Palette,
        TileBehaviors,
        Objects,
        Overlay,
        ItemBoxes,
        EndLine,
    }
    public class Offsets
    {
        static void RegTrack(int[][] _trackOffsets, byte[] rom, Track trackid, int address, int tilesetPointer, int palette, int tileBehaviours, int objects, int overlay, int itemBoxes, int endline)
        {
            int ptabs;
            int[] track;
            int[] TilePointers;
            track = _trackOffsets[(int)trackid];
            track[(int)TrackOffset.Address] = address;
            track[(int)TrackOffset.TilesetPointerTable] = tilesetPointer;
            ptabs /*abs pointer table ref*/ = track[(int)TrackOffset.Address] + track[(int)TrackOffset.TilesetPointerTable];
            TilePointers = new int[]{
                (ushort)(rom[(ptabs+1)]<<8|rom[ptabs  ]),
                (ushort)(rom[(ptabs+3)]<<8|rom[ptabs+2]),
                (ushort)(rom[(ptabs+5)]<<8|rom[ptabs+4]),
                (ushort)(rom[(ptabs+7)]<<8|rom[ptabs+6]),
            };
            track[(int)TrackOffset.TrackLayoutPointer] = rom[ptabs + 0x100 + 1] << 8 | rom[ptabs + 0x100];
            track[(int)TrackOffset.Tiles1] = ptabs + TilePointers[0];
            track[(int)TrackOffset.Tiles2] = ptabs + TilePointers[1];
            track[(int)TrackOffset.Tiles3] = ptabs + TilePointers[2];
            track[(int)TrackOffset.Tiles4] = ptabs + TilePointers[3];
            track[(int)TrackOffset.Palette] = address+palette;
            track[(int)TrackOffset.TileBehaviors] = address+tileBehaviours;
            track[(int)TrackOffset.Objects] = address + objects;
            track[(int)TrackOffset.Overlay] = address + overlay;
            track[(int)TrackOffset.ItemBoxes] = address + itemBoxes;
            track[(int)TrackOffset.EndLine] = address + endline;
            _trackOffsets[(int)trackid] = track;
        }
        public int[][] trackoffsets;
        public Offsets(byte[] rom)
        {
            trackoffsets = new int[Enum.GetValues(typeof(Track)).Length][];
            for (int i = 0; i < Enum.GetValues(typeof(Track)).Length; i++)
            {
                trackoffsets[i] = new int[Enum.GetValues(typeof(TrackOffset)).Length];
            }
            // commented tracks have broken adresses
            //|                        |        Track           | addr   |tilesetptr
            RegTrack(trackoffsets, rom, Track.SNESMarioCircuit1,0x2580d4, 0x00000de4, 0x000029b0, 0x00002a30, 0x00002f94, 0x00002f00, 0x00003064, 0x00003078);
            RegTrack(trackoffsets, rom, Track.SNESDonutPlains1, 0x25b17c, 0x00001470, 0x00002a34, 0x00002ab4, 0x000030f0, 0x00003000, 0x000031c0, 0x000031d4);
            RegTrack(trackoffsets, rom, Track.SNESGhostValley1, 0x25e380, 0x00000c38, 0x000018d8, 0x00001958, 0x00001ebc, 0x00001e00, 0x00001f8c, 0x00001fa4);
            RegTrack(trackoffsets, rom, Track.SNESBowserCastle1,0x260354, 0x00000e38, 0x00002140, 0x000021c0, 0x000027b4, 0x00002700, 0x00002884, 0x00002898);
            //RegTrack(trackoffsets, rom, Track.SNESMarioCircuit2,0x262c1c, 0x00000fa4, 0x00000fa4, 0x00001024, 0x00001618, 0x00001600, 0x000016e8, 0x000016fc);
            RegTrack(trackoffsets, rom, Track.SNESChocoIsland1, 0x264348, 0x00000ff8, 0x000024a4, 0x00002524, 0x0000298c, 0x00002900, 0x00002a5c, 0x00002a70);
            //RegTrack(trackoffsets, rom, Track.SNESGhostValley2, 0x266de8, 0x00000c58, 0x00000c58, 0x00000cd8, 0x00001434, 0x00001400, 0x00001504, 0x00001520);
            //RegTrack(trackoffsets, rom, Track.SNESDonutPlains2, 0x268338, 0x00001558, 0x00001558, 0x000015d8, 0x00001c14, 0x00001c00, 0x00001ce4, 0x00001cf8);
            //RegTrack(trackoffsets, rom, Track.SNESBowserCastle2,0x26a060, 0x00000fb8, 0x00000fb8, 0x00001038, 0x00001890, 0x00001800, 0x00001960, 0x00001974);
            //RegTrack(trackoffsets, rom, Track.SNESMarioCircuit3,0x26ba04, 0x000010fc, 0x000010fc, 0x0000117c, 0x00001824, 0x00001800, 0x000018f4, 0x00001908);
            RegTrack(trackoffsets, rom, Track.SNESKoopaBeach1,  0x26d33c, 0x00001328, 0x00002410, 0x00002490, 0x00002940, 0x00002900, 0x00002a10, 0x00002a24);
            //RegTrack(trackoffsets, rom, Track.SNESChocoIsland2, 0x26fd90, 0x0000123c, 0x0000123c, 0x000012bc, 0x00001790, 0x00001700, 0x00001860, 0x00001874);
            RegTrack(trackoffsets, rom, Track.SNESVanillaLake1, 0x271634, 0x000011a0, 0x0000205c, 0x000020dc, 0x00002520, 0x00002500, 0x000025f0, 0x00002604);
            //RegTrack(trackoffsets, rom, Track.SNESBowserCastle3,0x273c68, 0x00001030, 0x00001030, 0x000010b0, 0x0000177c, 0x00001700, 0x0000184c, 0x00001860);
            //RegTrack(trackoffsets, rom, Track.SNESMarioCircuit4,0x2754f8, 0x00001114, 0x00001114, 0x00001194, 0x00001860, 0x00001800, 0x00001930, 0x00001944);
            //RegTrack(trackoffsets, rom, Track.SNESDonutPlains3, 0x276e6c, 0x000015ec, 0x000015ec, 0x0000166c, 0x00001cf0, 0x00001c00, 0x00001dc0, 0x00001dd4);
            //RegTrack(trackoffsets, rom, Track.SNESKoopaBeach2,  0x278c70, 0x000014a4, 0x000014a4, 0x00001524, 0x00001944, 0x00001900, 0x00001a14, 0x00001a24);
            //RegTrack(trackoffsets, rom, Track.SNESGhostValley3, 0x27a6c4, 0x00000cc0, 0x00000cc0, 0x00000d40, 0x00001508, 0x00001500, 0x000015d8, 0x000015ec);
            //RegTrack(trackoffsets, rom, Track.SNESVanillaLake2, 0x27bce0, 0x000011f4, 0x000011f4, 0x00001274, 0x00001790, 0x00001700, 0x00001860, 0x00001874);
            // throws other error RegTrack(trackoffsets, rom, Track.SNESRainbowRoad,  0x27d584, 0x00000c88, 0x000016e4, 0x00001764, 0x00001e78, 0x00001e00, 0x00001f48, 0x00001f5c);
            //RegTrack(trackoffsets, rom, Track.SNESBattleCourse1,0x27f510, 0x00000a8c, 0x00000a8c, 0x00000c8c, 0x00000000, 0x00000000, 0x00000000, 0x00000000);
            //RegTrack(trackoffsets, rom, Track.SNESBattleCourse2,0x280580, 0x00000ac0, 0x00000ac0, 0x00000cc0, 0x00000000, 0x00000000, 0x00000000, 0x00000000);
            //RegTrack(trackoffsets, rom, Track.SNESBattleCourse3,0x281624, 0x00000a40, 0x00000a40, 0x00000c40, 0x00000000, 0x00000000, 0x00000000, 0x00000000);
            //RegTrack(trackoffsets, rom, Track.SNESBattleCourse4,0x282c24, 0x00000afc, 0x00000afc, 0x00000cfc, 0x00000000, 0x00000000, 0x00000000, 0x00000000);
            RegTrack(trackoffsets, rom, Track.PeachCircuit,     0x283d04, 0x000036c8, 0x000052a0, 0x00005320, 0x000061b8, 0x00006200, 0x000062d0, 0x000062ec);
            RegTrack(trackoffsets, rom, Track.ShyGuyBeach,      0x28a020, 0x00003324, 0x00004c54, 0x00004cd4, 0x000062c4, 0x00006300, 0x000063dc, 0x000063fc);
            RegTrack(trackoffsets, rom, Track.SunsetWilds,      0x29044c, 0x00006a10, 0x00008448, 0x000084c8, 0x0000a51c, 0x0000a500, 0x0000a634, 0x0000a660);
            RegTrack(trackoffsets, rom, Track.BowserCastle1,    0x29aadc, 0x000025e0, 0x000038c4, 0x00003944, 0x00005058, 0x00005000, 0x0000514c, 0x00005168);
            RegTrack(trackoffsets, rom, Track.LuigiCircuit,     0x29fc74, 0x00003e10, 0x00005c40, 0x00005cc0, 0x00006dfc, 0x00006e00, 0x00006f24, 0x00006f44);
            RegTrack(trackoffsets, rom, Track.RiversidePark,    0x2a6be8, 0x00004804, 0x0000639c, 0x0000641c, 0x00007720, 0x00007700, 0x00007848, 0x00007870);
            RegTrack(trackoffsets, rom, Track.YoshiDesert,      0x2ae488, 0x00003e04, 0x000058c0, 0x00005940, 0x000072c0, 0x00007300, 0x00007408, 0x00007430);
            //RegTrack(trackoffsets, rom, Track.BowserCastle2,    0x2b58e8, 0x000029e4, 0x000029e4, 0x00002a64, 0x00003580, 0x00003500, 0x00003674, 0x00003690);
            RegTrack(trackoffsets, rom, Track.MarioCircuit,     0x2b8fa8, 0x00002f88, 0x00004ee0, 0x00004f60, 0x00006058, 0x00006000, 0x00006174, 0x00006194);
            //RegTrack(trackoffsets, rom, Track.CheepCheepIsland, 0x2bf16c, 0x00003f14, 0x00003f14, 0x00003f94, 0x00005b64, 0x00005b00, 0x00005c78, 0x00005ca0);
            RegTrack(trackoffsets, rom, Track.RibbonRoad,       0x2c4e3c, 0x00003528, 0x0000459c, 0x0000461c, 0x00006434, 0x00006400, 0x00006564, 0x00006588);
            //RegTrack(trackoffsets, rom, Track.BowserCastle3,    0x2cb3f4, 0x00002a60, 0x00002a60, 0x00002ae0, 0x000037ac, 0x00003700, 0x000038c0, 0x000038d8);
            RegTrack(trackoffsets, rom, Track.SnowLand,         0x2cecfc, 0x00003a08, 0x00004fbc, 0x0000503c, 0x000070c8, 0x00007100, 0x000071ec, 0x00007214);
            RegTrack(trackoffsets, rom, Track.BooLake,          0x2d5f40, 0x00002e20, 0x0000487c, 0x000048fc, 0x00005a04, 0x00005a00, 0x00005b1c, 0x00005b40);
            RegTrack(trackoffsets, rom, Track.CheeseLand,       0x2dbab0, 0x00003d14, 0x00005574, 0x000055f4, 0x00006d10, 0x00006d00, 0x00006e3c, 0x00006e60);
            RegTrack(trackoffsets, rom, Track.RainbowRoad,      0x2e2940, 0x00002cac, 0x00003bfc, 0x00003c7c, 0x0000526c, 0x00005200, 0x0000536c, 0x00005398);
            RegTrack(trackoffsets, rom, Track.SkyGarden,        0x2e7d08, 0x00003944, 0x0000568c, 0x0000570c, 0x000069b8, 0x00006a00, 0x00006ae0, 0x00006b00);
            //RegTrack(trackoffsets, rom, Track.BrokenPier,       0x2ee838, 0x00002e64, 0x00002e64, 0x00002ee4, 0x00003df4, 0x00003e00, 0x00003f24, 0x00003f4c);
            //RegTrack(trackoffsets, rom, Track.BowserCastle4,    0x2f27b4, 0x00003260, 0x00003260, 0x000032e0, 0x000040f0, 0x00004100, 0x00004214, 0x0000423c);
            //RegTrack(trackoffsets, rom, Track.LakesidePark,     0x2f6a20, 0x00004b80, 0x00004b80, 0x00004c00, 0x00006470, 0x00006400, 0x000065a4, 0x000065c4);
            //RegTrack(trackoffsets, rom, Track.BattleCourse1,    0x2fd014, 0x00000c2c, 0x00000c2c, 0x00000cac, 0x000011c8, 0x00000000, 0x000011cc, 0x000011f4);
            //RegTrack(trackoffsets, rom, Track.BattleCourse2,    0x2fe234, 0x00000c80, 0x00000c80, 0x00000d00, 0x000010fc, 0x00000000, 0x00001100, 0x00001118);
            //RegTrack(trackoffsets, rom, Track.BattleCourse3,    0x2ff378, 0x00001124, 0x00001124, 0x000011a4, 0x0000157c, 0x00000000, 0x00001580, 0x000015a4);
            //RegTrack(trackoffsets, rom, Track.BattleCourse4,    0x300948, 0x00000b9c, 0x00000b9c, 0x00000c1c, 0x00000e68, 0x00000000, 0x00000e6c, 0x00000e84);
            //RegTrack(trackoffsets, rom, Track.TestTrack,        0x3017f8, 0x000010fc, 0x000010fc, 0x0000117c, 0x00001644, 0x00001600, 0x0000164c, 0x00001650);
            System.Diagnostics.Debug.WriteLine("Generated offsets");
        }

        public int this[Track offset, TrackOffset trackOffset]
        {
            get => trackoffsets[(int)offset][(int)trackOffset];
            set => trackoffsets[(int)offset][(int)trackOffset] = value;
        }
        public int[] this[int offsets]
        {
            get => trackoffsets[offsets];
            set => trackoffsets[offsets] = value;
        }
    }
}