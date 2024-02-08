using static SDL2.SDL;

namespace MkscEdit.Types {
    public class Tile
    {
        public Palette palette;
        public byte[,] indicies;
        public Tile(byte[,] indicies, Palette palette)
        {
            this.palette = palette;
            this.indicies = indicies;
        }

        public static Tile[] GenerateTiles(byte[] indicies, Palette palette)
        {
            int tileno = (int)Math.Ceiling((decimal)indicies.Length / (decimal)64);
            Tile[] tiles = new Tile[tileno];
            for (int t = 0; t < tileno; t++)
            {
                byte[,] tile = new byte[8, 8];
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if ((y * 8 + x) < indicies.Length)
                        {
                            tile[x, y] = indicies[y * 8 + (x + t * 64)];
                        }
                        else
                        {
                            tile[x, y] = 0;
                        }
                    }
                }
                tiles[t] = new Tile(tile, palette);
            }
            return tiles;
        }
        public unsafe SDL_Surface* ToImage()
        {
            SDL_Surface* surface = (SDL_Surface*)SDL_CreateRGBSurface(0,8,8,32,0,0,0,0).ToPointer();

            for (int y = 0;y < 8; y++)
            {
                for(int x = 0;x < 8; x++){
                    uint pixel = palette[indicies[x,y]].ToColor();
                    uint* target_pixel = (uint*) ((byte*)surface->pixels
                                             + y * surface->pitch
                                             + x * ((SDL_PixelFormat*)surface->format.ToPointer())->BytesPerPixel);
                    *target_pixel = pixel;
                }
            }
            return surface;
        }
    }
}