using MkscEdit.Compression;
using MkscEdit.Extract;
using MkscEdit.UI;
using static SDL2.SDL;

namespace MkscEdit
{
    class TrackEditor
    {
        TilePalette tilePalette;
        TilePanel tilemap;
        Tile tile;
        int selectedTile = 0;
        Track track = Track.PeachCircuit;
        public TrackEditor()
        {
            SDL_Rect elementPosition = new SDL_Rect() { x = Program.WindowWidth - 256, y = 0, w = 256, h = 256 };
            tilePalette = new TilePalette(elementPosition, new(Program.WindowWidth - 256, 0));
            tilePalette.SetTrack(track);

            elementPosition = new SDL_Rect() { x = 0, y = 0, w = Program.WindowWidth - 256, h = Program.WindowHeight };
            tilemap = new TilePanel(elementPosition, new(0, 0));
            tilemap.tileSize = 4;
            tilemap.SetTrack(track);
            tile = new Tile(new(0,0));
            tile.SetTrack(track);
            byte[] layout = new byte[256*256];
            int currentOffset = 0;
            foreach (var o in Program.offsets[track].LayoutBlocks)
            {
                var b = Program.rom.DecompressRange(Program.file, o);
                Array.Copy(b, 0, layout, currentOffset, b.Length);
                currentOffset += b.Length;
            }
            
            byte[,] output = new byte[256, 256];
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    output[i, j] = layout[i * 256 + j];
                }
            }
            tilemap.indicies = output;
        }
        public void Update()
        {
            int x, y;
            tilemap.ElementPosition = new SDL_Rect() { x = 0, y = 0, w = Program.WindowWidth - 256, h = Program.WindowHeight };
            tilePalette.ElementPosition = new SDL_Rect() { x = Program.WindowWidth - 256, y = 0, w = 256, h = 256 };
            tilePalette.ContentPosition = new(Program.WindowWidth - 256, 0);
            SDL_GetMouseState(out x, out y);
            tile.ContentPosition = new(x,y);
            if (selectedTile!=-1)
            {
                tile.SetTile((byte)selectedTile);
            }
        }
        public void Draw()
        {
            tilemap.DrawElement();
            tilePalette.DrawElement();

            tile.DrawElement();
        }
        public void MouseDown(SDL_Event e)
        {
            selectedTile = tilePalette.GetTile(e.motion.x, e.motion.y);
        }
        void TestClick()
        {
            Console.WriteLine("Clicked!");
        }
    }
}
