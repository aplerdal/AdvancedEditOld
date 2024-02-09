using System.Drawing;
using MkscEdit.Extract;
using MkscEdit.Types;
using static SDL2.SDL;
namespace MkscEdit{
    class TilePalette : TilePanel{
        
        public TilePalette(IntPtr renderer, Rom rom, SDL_Rect elementPosition, Point contentPosition) : base(renderer,rom,elementPosition,contentPosition) {
            indicies = new byte[16,16];
            for (int i = 0; i<256; i++){
                indicies[(int)(i/16),(int)(i%16)] = (byte)i;
            }
        }
        public new void DrawElement(){
            SDL_RenderSetClipRect(renderer,ref ElementPosition);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = x * 8 + y * 8 * columns, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * tileSize + ContentPosition.X, y = y * tileSize + ContentPosition.Y, w = tileSize, h = tileSize };
                    //SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref d);
                    SDL_RenderCopy(renderer, tileAtlas, ref s, ref d);
                }
            }
            SDL_RenderSetClipRect(renderer, IntPtr.Zero);
        }
    }
}