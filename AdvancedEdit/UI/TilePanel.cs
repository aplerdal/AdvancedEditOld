using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AdvancedEdit.UI
{
    class TilePanel
    {
        public byte[,] indicies;
        public TrackId trackId;
        public Texture2D[] tiles;
        public int tileSize;
        public Vector2I mapSize;

        // The position of the tiles relative to the current imgui window
        public Vector2I contentPosition;

        public TilePanel()
        {
            indicies = new byte[0, 0];
            tiles = new Texture2D[256];
        }

        /// <summary>
        /// Sets the TilePanel's tiles and palette to the given track's
        /// </summary>
        /// <param name="trackId">Id of new track</param>
        public void SetTrack(TrackId trackId)
        {
            this.trackId = trackId;
            for (int i = 0; i < AdvancedEditor.tracks[(int)trackId].Tiles.Length; i++)
            {
                //Load Tile texture
                Texture2D tile = AdvancedEditor.tracks[(int)trackId].Tiles[i].ToImage(AdvancedEditor.gd);

                tiles[i] = tile;
            }
        }
        /// <summary>
        /// Gets the tile at the given position. Must be called from inside the UI rendering sequence of the TilePanel
        /// </summary>
        /// <param name="position">The absolute to fetch</param>
        /// <returns></returns>
        public int GetTile(Vector2 position)
        {
            var winPos = ImGui.GetWindowPos();
            var winSize = ImGui.GetWindowSize();
            var relPosition = position - winPos;
            if (ImGui.IsWindowHovered())
            {
                if ((relPosition.X >= contentPosition.X) &&
                    (relPosition.Y >= contentPosition.Y) &&
                    (relPosition.X <= (contentPosition.X + tileSize*mapSize.X)) && 
                    (relPosition.Y <= (contentPosition.Y + tileSize*mapSize.Y)))
                {
                    int tilex = (int)Math.Floor((decimal)(relPosition.X - contentPosition.X) / tileSize);
                    int tiley = (int)Math.Floor((decimal)(relPosition.Y - contentPosition.Y) / tileSize);
                    int temp = tilex + tiley * mapSize.X;
                    return indicies[(int)temp / 16, temp % 16];
                }
            }
            return -1;
        }
        public void SetTile(int idx, Vector2 position)
        {
            var winPos = ImGui.GetWindowPos();
            var winSize = ImGui.GetWindowSize();
            var relPosition = position - winPos;
            if (idx == -1) return;
            if (ImGui.IsWindowHovered())
            {
                if ((relPosition.X >= contentPosition.X) &&
                    (relPosition.Y >= contentPosition.Y) &&
                    (relPosition.X <= (contentPosition.X + tileSize * mapSize.X)) &&
                    (relPosition.Y <= (contentPosition.Y + tileSize * mapSize.Y)))
                {
                    int tilex = (int)Math.Floor((decimal)(relPosition.X - contentPosition.X) / tileSize);
                    int tiley = (int)Math.Floor((decimal)(relPosition.Y - contentPosition.Y) / tileSize);
                    int temp = tilex + tiley * mapSize.X;
                    indicies[(int)temp / 16, temp % 16] = (byte)idx;
                }
            }
        }
        public void Draw()
        {
            /* TODO
            ImGui.Begin("TilePanel");
            mapSize = new Vector2I(indicies.GetLength(0), indicies.GetLength(1));
            Texture2D renderTexture = new Texture2D(AdvancedEditor.gd, (int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = indicies[y, x] * 8, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * tileSize + ContentPosition.X, y = y * tileSize + ContentPosition.Y, w = tileSize, h = tileSize };
                    SDL_RenderCopy(Program.Renderer, tileAtlas, ref s, ref d);
                }
            }*/ 
        }
    }
}
