using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
using AdvancedEdit.UI;
using System.Diagnostics;
using System.Drawing;
using static SDL2.SDL;

namespace AdvancedEdit
{
    class TrackEditor
    {
        TilePalette tilePalette;
        TilePanel tilemap;
        UITile tile;
        int selectedTile = 0;
        TrackId track = TrackId.PeachCircuit;
        bool tilemapDragged = false;
        bool leftDown = false;

        private Point mousePosition = new(0, 0);

        UIManager uiManager;

        public TrackEditor()
        {
            uiManager = new UIManager();
            var button = new Dropdown<int>(
                new SDL_Rect(Program.WindowWidth - 256, 256, 256, 32),
                new SDL_Color(64,64,64,255),
                new List<int>() { 0,1,2,3 }
            );;
            uiManager.AddElement(button);

            SDL_Rect elementPosition = new SDL_Rect(Program.WindowWidth - 256,0,256,256);
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
            uiManager.UpdateElements();
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
            if (leftDown){
                if(tilemap.ElementPosition.Contains(x,y)){
                    tilemap.SetTile((byte)selectedTile,x,y);
                }
            }
            if (Program.keyDown[(int)SDL_Keycode.SDLK_LCTRL] && Program.keyPress[(int)SDL_Keycode.SDLK_EQUALS])
            {
                track = track.Next();
                while (Program.tracks[(int)track].Tiles == null) track = track.Next();
                tilemap.SetTrack(track);
                tilemap.indicies = Program.tracks[(int)track].Indicies;
                tilePalette.SetTrack(track);
                tile.SetTrack(track);
            }
            if (Program.keyDown[(int)SDL_Keycode.SDLK_LCTRL] && Program.keyPress[(int)SDL_Keycode.SDLK_MINUS])
            {
                track = track.Previous();
                while (Program.tracks[(int)track].Tiles == null) track = track.Previous();
                tilemap.SetTrack(track);
                tilemap.indicies = Program.tracks[(int)track].Indicies;
                tilePalette.SetTrack(track);
                tile.SetTrack(track);
            }
        }
        public void Close()
        {
            Program.tracks[(int)track].PackData();
        }
        public void Events(SDL_Event e)
        {
            uiManager.ElementEvents(e);
            switch (e.type)
            {
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    MouseUp(e);
                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    MouseDown(e);
                    break;
                case SDL_EventType.SDL_MOUSEWHEEL:
                    ScrollWheel(e);
                    break;
                case SDL_EventType.SDL_MOUSEMOTION:
                    MouseMotion(e);
                    break;
            }
        }
        public void Draw()
        {
            uiManager.DrawElements();
            tilemap.Draw();
            tilePalette.Draw();

            tile.Draw();
            
        }
        public void MouseMotion(SDL_Event e)
        {
            if (tilemapDragged == true)
            {
                tilemap.ContentPosition = new(tilemap.ContentPosition.X + e.motion.xrel, tilemap.ContentPosition.Y + e.motion.yrel);
            }
            mousePosition = new(e.motion.x, e.motion.y);
        }
        public void MouseDown(SDL_Event e)
        {
            if (e.button.button == SDL_BUTTON_LEFT)
            {
                leftDown = true;
                if (tilePalette.GetTile(e.motion.x, e.motion.y) > -1)
                {
                    selectedTile = tilePalette.GetTile(e.motion.x, e.motion.y);
                }
            }
            if (e.button.button == SDL_BUTTON_MIDDLE)
            {
                tilemapDragged = true;
            }
        }
        public void MouseUp(SDL_Event e)
        {
            if (e.button.button == SDL_BUTTON_LEFT)
            {
                leftDown = false;
            }
            if (e.button.button == SDL_BUTTON_MIDDLE)
            {
                tilemapDragged = false;
            }
        }


        public void ScrollWheel(SDL_Event e)
        {
            if (e.wheel.y == 0) return;

            var scroll = e.wheel.y / Math.Abs(e.wheel.y);
            double oldSize = tilemap.tileSize;
            tilemap.tileSize = Math.Clamp(tilemap.tileSize + scroll, 2, 32);

            double scaleFactor = tilemap.tileSize / oldSize -1;

            var offsetX = mousePosition.X  * scaleFactor;
            var offsetY = mousePosition.Y * scaleFactor;

            tilemap.ContentPosition = new Point
            (
                (int)(tilemap.ContentPosition.X * (scaleFactor+1) - offsetX),
                (int)(tilemap.ContentPosition.Y * (scaleFactor+1) - offsetY)
            );

            
        }
    }
}
