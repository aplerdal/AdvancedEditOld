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
        public IntPtr[] tileTextures = new IntPtr[256];
        public int tileSize = 16;
        public Vector2I mapSize = new(16,16);
        public byte selectedTile;

        //ImGui items
        private int brushSize;
        private bool ratioLocked;

        public TilePalette()
        {
            for (int i = 0; i<256; i++){
                indicies[(int)(i/16),(int)(i%16)] = (byte)i;
            }
            selectedTile = 0;
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
                if (tileTextures[i] != IntPtr.Zero){
                    AdvancedEditor.GuiRenderer.UnbindTexture(tileTextures[i]);
                }
                Texture2D tile = AdvancedEditor.tracks[(int)trackId].Tiles[i].ToImage(AdvancedEditor.gd);
                tileTextures[i] = AdvancedEditor.GuiRenderer.BindTexture(tile);

                tiles[i] = tile;
            }
        }
        public void Draw()
        {
            ImGui.Begin("TilePanel");
            var usableArea = ImGui.GetWindowSize() - 2 * ImGui.GetCursorPos();
            var areaStart = ImGui.GetCursorPos();
            Vector2I layout = new((int)Math.Floor(usableArea.X/tileSize), (int)Math.Ceiling(256/Math.Floor(usableArea.X/tileSize)));

            float windowVisibleX = ImGui.GetWindowPos().X + ImGui.GetContentRegionMax().X;
            ImGuiStylePtr style = ImGui.GetStyle();

            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(0, 0));
            for (int i = 0; i < 256; i++){
                if (ImGui.ImageButton($"tilepal{i}", tileTextures[i], new System.Numerics.Vector2(tileSize, tileSize)))
                {
                    selectedTile = (byte)i;
                }
                float lastButtonX = ImGui.GetItemRectMax().X;
                float nextButtonX = lastButtonX + style.ItemSpacing.X + new System.Numerics.Vector2(tileSize, tileSize).X;
                if (ratioLocked)
                {
                    if (i % 16 != 15 && i!=256)
                        ImGui.SameLine();
                }
                else
                {
                    if (i + 1 < 256 && nextButtonX < windowVisibleX && i != 256)
                        ImGui.SameLine();
                }
            }
            ImGui.PopStyleVar();
            ImGui.PopStyleVar();
            ImGui.SeparatorText("");
            ImGui.Checkbox("Palette always square?", ref ratioLocked);
            ImGui.SeparatorText("Brush Settings");
            ImGui.InputInt("Brush Size", ref brushSize);
            brushSize = Math.Clamp(brushSize, 1, 256);
            ImGui.SeparatorText("Preview");
            ImGui.Text("Active Tile");
            ImGui.Image(tileTextures[selectedTile], new System.Numerics.Vector2(64, 64));

            ImGui.End();
        }
    }
}