#region LICENSE
/*
Copyright(C) 2024 Andrew Lerdal

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
#endregion

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
        //public Texture2D[] tiles = new Texture2D[256];
        //public IntPtr[] tileTextures = new IntPtr[256];
        public int tileSize = 16;
        System.Numerics.Vector2 padding = new System.Numerics.Vector2(0,0);
        public Vector2I mapSize = new(16,16);
        //public byte selectedTile;

        //ImGui items
        private int brushSize;
        private bool ratioLocked;

        public TilePalette()
        {
        }
        public void Draw(IntPtr[] tileTextures, ref byte selectedTile)
        {
            ImGui.Begin("Brush");

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