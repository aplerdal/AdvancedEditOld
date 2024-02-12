using System.Drawing;
using MkscEdit.Extract;
using MkscEdit.Types;
using static SDL2.SDL;
namespace MkscEdit.UI{
    class TilePanel{
        public byte[,] indicies;
        public TrackId trackId;
        public IntPtr tileAtlas;
        public SDL_Rect ElementPosition;
        public Point ContentPosition;
        public int tileSize = 16;
        public int columns;
        public int rows;
        
        public TilePanel(SDL_Rect elementPosition, Point contentPosition) {
            indicies = new byte[0,0];
            tileAtlas = SDL_CreateTextureFromSurface(Program.Renderer,SDL_CreateRGBSurface(0, 8, 2048, 32, 0, 0, 0, 0));
            ElementPosition = elementPosition;
            ContentPosition = contentPosition;
        }
        public unsafe void SetTrack(TrackId track){
            trackId = track;
            IntPtr ta = SDL_CreateRGBSurface(0,8,2048,32,0,0,0,0);
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
        public int GetTile(int x, int y){
            if (new SDL_Rect() {x=ContentPosition.X,y=ContentPosition.Y,w=tileSize*columns,h=tileSize*rows}.Contains(x,y)){
                int tilex = (int)Math.Floor((decimal)(x-ContentPosition.X)/tileSize);
                int tiley = (int)Math.Floor((decimal)(y-ContentPosition.Y)/tileSize);
                int temp = tilex + tiley * columns;
                return indicies[(int)temp/16, temp%16];
            } else {
                return -1;
            }
        }
        public void SetTile(byte idx, int x, int y) { 
            if (new SDL_Rect() {x=ContentPosition.X,y=ContentPosition.Y,w=tileSize*columns,h=tileSize*rows}.Contains(x,y)){
                int tilex = (int)Math.Floor((decimal)(x-ContentPosition.X)/tileSize);
                int tiley = (int)Math.Floor((decimal)(y-ContentPosition.Y)/tileSize);
                int temp = tilex + tiley * 256;
                indicies[(int)temp / 256, temp%256] = (byte)idx;
            }
        }
        public void DrawElement(){
            columns = /*(int)Math.Clamp(Math.Ceiling((decimal)ElementPosition.w / tileSize), 0, */indicies.GetLength(0);
            rows = /*(int)Math.Clamp(Math.Ceiling((decimal)ElementPosition.h / tileSize),0,*/indicies.GetLength(1);

            SDL_RenderSetClipRect(Program.Renderer,ref ElementPosition);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = indicies[y,x]*8, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * tileSize + ContentPosition.X, y = y * tileSize + ContentPosition.Y, w = tileSize, h = tileSize };
                    SDL_RenderCopy(Program.Renderer, tileAtlas, ref s, ref d);
                }
            }
            SDL_RenderSetClipRect(Program.Renderer, IntPtr.Zero);
        }
    }
}