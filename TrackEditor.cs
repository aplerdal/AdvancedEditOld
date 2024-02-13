using MkscEdit.Compression;
using MkscEdit.TrackData;
using MkscEdit.Types;
using MkscEdit.UI;
using static SDL2.SDL;

namespace MkscEdit
{
    class TrackEditor
    {
        TilePalette tilePalette;
        TilePanel tilemap;
        UITile tile;
        int selectedTile = 0;
        TrackId track = TrackId.PeachCircuit;
        bool tilemapDragged = false;

        public TrackEditor()
        {
            SDL_Rect elementPosition = new SDL_Rect() { x = Program.WindowWidth - 256, y = 0, w = 256, h = 256 };
            tilePalette = new TilePalette(elementPosition, new(Program.WindowWidth - 256, 0));
            tilePalette.SetTrack(track);

            elementPosition = new SDL_Rect() { x = 0, y = 0, w = Program.WindowWidth - 256, h = Program.WindowHeight };
            tilemap = new TilePanel(elementPosition, new(0, 0));
            tilemap.tileSize = 4;
            tilemap.SetTrack(track);
            tilemap.indicies = Program.tracks[(int)track].Indicies;
            tile = new UITile(new(0,0));
            tile.SetTrack(track);
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
        public void Close()
        {
            Program.tracks[(int)track].PackData();
        }
        public void Draw()
        {
            tilemap.DrawElement();
            tilePalette.DrawElement();

            tile.DrawElement();
        }
        public void MouseMotion(SDL_Event e)
        {
            if (tilemapDragged == true)
            {
                tilemap.ContentPosition = new(tilemap.ContentPosition.X + e.motion.xrel, tilemap.ContentPosition.Y + e.motion.yrel);
            }
        }
        public void MouseDown(SDL_Event e)
        {
            if (e.button.button == SDL_BUTTON_LEFT)
            {
                if (tilePalette.GetTile(e.motion.x, e.motion.y) > -1)
                {
                    selectedTile = tilePalette.GetTile(e.motion.x, e.motion.y);
                }
                if (tilemap.ElementPosition.Contains(e.motion.x, e.motion.y))
                {
                    tilemap.SetTile((byte)selectedTile, e.motion.x, e.motion.y);
                }
            }
            if (e.button.button == SDL_BUTTON_MIDDLE)
            {
                tilemapDragged = true;
            }
        }
        public void MouseUp(SDL_Event e)
        {
            if (e.button.button == SDL_BUTTON_MIDDLE)
            {
                tilemapDragged = false;
            }
        }
        public void ScrollWheel(SDL_Event e)
        {
            if (e.wheel.y != 0)
            {
                tilemap.tileSize += e.wheel.y / Math.Abs(e.wheel.y);
                tilemap.tileSize = Math.Clamp(tilemap.tileSize, 1, 32);
            }   
        }
    }
}
