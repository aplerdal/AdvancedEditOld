using AdvancedEdit.TrackData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace AdvancedEdit.UI
{
    class UITile : UIElement
    {
        public TrackId trackId;
        public IntPtr tileAtlas;
        public Point ContentPosition;
        public byte tileIdx = 0;
        public int tileSize = 16;

        public UITile(Point contentPosition)
        {
            tileAtlas = SDL_CreateTextureFromSurface(Program.Renderer, SDL_CreateRGBSurface(0, 8, 2048, 32, 0, 0, 0, 0));
            ContentPosition = contentPosition;
        }
        public void SetTile(byte tileIndex)
        {
            tileIdx = tileIndex;
        }
        public unsafe void SetTrack(TrackId track)
        {
            trackId = track;
            IntPtr ta = SDL_CreateRGBSurface(0, 8, 2048, 32, 0, 0, 0, 0);
            for (int i = 0; i < Program.tracks[(int)track].Tiles.Length; i++)
            {
                var t = Program.tracks[(int)track].Tiles[i];
                IntPtr s = (IntPtr)t.ToImage();
                SDL_Rect d = new SDL_Rect { x = 0, y = i * 8, w = 8, h = 8 };
                SDL_BlitSurface(s, IntPtr.Zero, ta, ref d);
                SDL_FreeSurface(s);
            }
            SDL_DestroyTexture(tileAtlas);
            tileAtlas = SDL_CreateTextureFromSurface(Program.Renderer, ta);
        }
        public override void Update()
        {
            // No Updates
        }
        public override void Events(SDL_Event e)
        {
            // No events
        }
        public override void Draw()
        {
            SDL_Rect s = new SDL_Rect() { x = 0, y = tileIdx * 8, w = 8, h = 8 };
            SDL_Rect d = new SDL_Rect() { x = ContentPosition.X-tileSize/2, y = ContentPosition.Y-tileSize/2, w = tileSize, h = tileSize };
            SDL_RenderCopy(Program.Renderer, tileAtlas, ref s, ref d);
            SDL_RenderSetClipRect(Program.Renderer, IntPtr.Zero);
        }
    }
}
