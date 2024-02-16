using System.Drawing;
using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
using static SDL2.SDL;
namespace AdvancedEdit.UI{
    class TilePalette : TilePanel{
        public TilePalette( SDL_Rect elementPosition, Point contentPosition) : base(elementPosition,contentPosition) {
            indicies = new byte[16,16];
            columns = 16;
            rows = 16;
            for (int i = 0; i<256; i++){
                indicies[(int)(i/16),(int)(i%16)] = (byte)i;
            }
        }
        public override void Update()
        {
            // No Updates
        }
        public override void Events(SDL_Event e)
        {
            // No events
        }
        public override void Draw(){
            SDL_RenderSetClipRect(Program.Renderer,ref ElementPosition);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = x * 8 + y * 8 * columns, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * tileSize + ContentPosition.X, y = y * tileSize + ContentPosition.Y, w = tileSize, h = tileSize };
                    SDL_RenderCopy(Program.Renderer, tileAtlas, ref s, ref d);
                }
            }
            SDL_RenderSetClipRect(Program.Renderer, IntPtr.Zero);
        }
    }
}