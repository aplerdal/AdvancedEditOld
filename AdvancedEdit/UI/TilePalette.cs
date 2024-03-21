using AdvancedEdit.TrackData;
using AdvancedEdit.Types;

using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Numerics;

namespace AdvancedEdit.UI{
    class TilePalette{
        int[,] indicies = new int[16,16];
        public TrackId trackId;
        public Texture2D[] tiles = new Texture2D[256];
        public int tileSize = 8;
        public Vector2I mapSize = new(16,16);
        RenderTarget2D renderTarget = new RenderTarget2D(AdvancedEditor.gd, 16 * 8, 16 * 8);
        System.Numerics.Vector2 relPos;
        IntPtr targetPtr;

        public TilePalette()
        {
            for (int i = 0; i<256; i++){
                indicies[(int)(i/16),(int)(i%16)] = (byte)i;
            }
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
        public int GetTile(System.Numerics.Vector2 position)
        {
            var winPos = ImGui.GetWindowPos();
            var winSize = ImGui.GetWindowSize();
            var relPosition = position - winPos;
            if (ImGui.IsWindowHovered())
            {
                if ((relPosition.X >= 0) &&
                    (relPosition.Y >= 0) &&
                    (relPosition.X <= (tileSize*mapSize.X)) && 
                    (relPosition.Y <= (tileSize*mapSize.Y)))
                {
                    int tilex = (int)Math.Floor((decimal)relPosition.X/tileSize);
                    int tiley = (int)Math.Floor((decimal)relPosition.Y/tileSize);
                    int temp = tilex + tiley * mapSize.X;
                    return indicies[(int)temp / 16, temp % 16];
                }
            }
            return -1;
        }
        public void SetTile(int idx, System.Numerics.Vector2 position)
        {
            var winPos = ImGui.GetWindowPos();
            var winSize = ImGui.GetWindowSize();
            var relPosition = position - winPos;
            if (idx == -1) return;
            if (ImGui.IsWindowHovered())
            {
                if ((relPosition.X >= 0) &&
                    (relPosition.Y >= 0) &&
                    (relPosition.X <= (tileSize * mapSize.X)) &&
                    (relPosition.Y <= (tileSize * mapSize.Y)))
                {
                    int tilex = (int)Math.Floor((decimal)relPosition.X / tileSize);
                    int tiley = (int)Math.Floor((decimal)relPosition.Y / tileSize);
                    int temp = tilex + tiley * mapSize.X;
                    indicies[(int)temp / 16, temp % 16] = (byte)idx;
                }
            }
        }
        public static unsafe void Square(ImGuiSizeCallbackData* data)
        {
            data->DesiredSize.X = data->DesiredSize.Y = Math.Max(data->CurrentSize.X, data->CurrentSize.Y);
        }
        public void Draw()
        {
            ImGui.Begin("TilePanel");
            mapSize = new Vector2I(indicies.GetLength(0), indicies.GetLength(1));
            AdvancedEditor.gd.SetRenderTarget(renderTarget);
            AdvancedEditor.gd.Clear(Color.Transparent);

            AdvancedEditor.spriteBatch.Begin(samplerState: new SamplerState() { Filter=TextureFilter.MinLinearMagPointMipPoint});

            for (int x = 0; x < mapSize.X; x++)
            {
                for (int y = 0; y < mapSize.Y; y++)
                {
                    Texture2D tile = tiles[indicies[y, x]];
                    Rectangle dest = new Rectangle(x * tileSize,y * tileSize, tileSize, tileSize);
                    AdvancedEditor.spriteBatch.Draw(tile, dest, Color.White);
                }
            }
            
            AdvancedEditor.spriteBatch.End();
            AdvancedEditor.gd.SetRenderTarget(null);

            if (targetPtr != IntPtr.Zero) AdvancedEditor.GuiRenderer.UnbindTexture(targetPtr);
            targetPtr = AdvancedEditor.GuiRenderer.BindTexture(renderTarget);
            var Scale = ImGui.GetWindowSize() - 2 * ImGui.GetCursorPos();
            ImGui.Image(targetPtr, new System.Numerics.Vector2(Math.Min(Scale.X,Scale.Y), Math.Min(Scale.X, Scale.Y)));
            // get cursor pos as use as relative pos
            ImGui.End();
        }
    }
}