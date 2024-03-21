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
        public int tileSize = 8;
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
            //TODO Fix memory leak - F
            ImGui.Begin("TilePanel");
            mapSize = new Vector2I(indicies.GetLength(0), indicies.GetLength(1));
            RenderTarget2D renderTexture = new RenderTarget2D(AdvancedEditor.gd, (int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y);
            AdvancedEditor.gd.SetRenderTarget(renderTexture);
            AdvancedEditor.gd.Clear(Color.CornflowerBlue);

            AdvancedEditor.spriteBatch.Begin();

            for (int x = 0; x < mapSize.X; x++)
            {
                for (int y = 0; y < mapSize.Y; y++)
                {
                    Texture2D tile = tiles[indicies[y, x]];
                    Rectangle dest = new Rectangle(x * tileSize + contentPosition.X,y * tileSize + contentPosition.Y, tileSize, tileSize);
                    AdvancedEditor.spriteBatch.Draw(tile, dest, Color.White);
                }
            }
            
            AdvancedEditor.spriteBatch.End();
            AdvancedEditor.gd.SetRenderTarget(null);

            IntPtr targetPtr = AdvancedEditor.GuiRenderer.BindTexture(renderTexture);

            ImGui.SetCursorPos(ImGui.GetWindowPos());
            ImGui.Image(targetPtr, ImGui.GetWindowSize());
            ImGui.End();
        }
    }
}
