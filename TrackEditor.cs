using MkscEdit.Compression;
using MkscEdit.Extract;
using MkscEdit.UI;
using static SDL2.SDL;

namespace MkscEdit
{
    class TrackEditor
    {
        TextButton button;
        TilePalette tilePalette;
        TilePanel tilemap;
        Tile tile;
        int selectedTile = 0;
        Track track = Track.PeachCircuit;
        public TrackEditor()
        {
            Console.WriteLine(Program.WindowWidth);
            SDL_Rect elementPosition = new SDL_Rect() { x = Program.WindowWidth - 256, y = 0, w = 256, h = 256 };
            tilePalette = new TilePalette(elementPosition, new(Program.WindowWidth - 256, 0));
            tilePalette.SetTrack(track);
            elementPosition = new SDL_Rect() { x = 0, y = 0, w = Program.WindowWidth - 256, h = Program.WindowHeight };
            tilemap = new TilePanel(elementPosition, new(0, 0));
            tilemap.SetTrack(track);
            tile = new Tile(new(0,0));
            tile.SetTrack(track);

            byte[] b = Program.rom.DecompressRange(Program.file, Program.offsets[(int)track]+1);
            byte[,] output = new byte[512, 512];
            for (int i = 0; i < 512; i++)
            {
                for (int j = 0; j < 512; j++)
                {
                    output[i, j] = b[i * 512 + j];
                }
            }
            tilemap.indicies = output;
        }
        public void Update()
        {
            int x, y;
            SDL_GetMouseState(out x, out y);
            tile.ContentPosition = new(x,y);
            if (selectedTile!=-1)
            {
                tile.SetTile((byte)selectedTile);
            }
        }
        public void Draw()
        {
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
