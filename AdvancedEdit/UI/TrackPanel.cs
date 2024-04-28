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
using MonoGame.ImGuiNet;
using System;

namespace AdvancedEdit.UI
{
    class TrackPanel
    {
        int tileSize = 16;
        Vector2I mapSize;
        bool dragged = false;
        Vector2 lastPos;
        Vector2 contentPosition = new Vector2(0,0);
        ToolManager toolManager = new ToolManager();
        public TrackPanel()
        {
            
        }
        /// <summary>
        /// Gets the tile position in indicies at the given absolute position. Must be called from inside the UI rendering sequence of the TilePanel
        /// </summary>
        /// <param name="position">The absolute to fetch</param>
        /// <returns></returns>
        public Vector2I GetTilePos(Vector2 position)
        {
            throw new NotImplementedException();
        }
        public void SetTile(int selectedTile, Vector2 position)
        {
            throw new NotImplementedException();
        }
        public void Draw(IntPtr[] tileTextures, byte[,] indicies, ref byte selectedTile)
        {
            Vector2 newContentPos = contentPosition;
            ImGui.Begin("TilePanel",ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoScrollbar);
            mapSize = new Vector2I(indicies.GetLength(0), indicies.GetLength(1));
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new System.Numerics.Vector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(0, 0));
            for (int y = 0; y < mapSize.Y; y++)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    ImGui.SetCursorPos(new(x*tileSize+contentPosition.X,y*tileSize+contentPosition.Y));
                    ImGui.Image(tileTextures[indicies[y,x]], new System.Numerics.Vector2(tileSize, tileSize));
                    if (previewIndicies[y,x]!=-1) toolManager.DrawToolPreview(x,y);
                    if (ImGui.IsItemHovered())
                    {
                        if (ImGui.IsMouseDown(ImGuiMouseButton.Left))
                        {
                            indicies[y, x] = selectedTile;
                        } else {

                        }
                    }
                    if (ImGui.IsMouseDown(ImGuiMouseButton.Middle))
                    {
                        if (ImGui.IsItemHovered()) 
                        {
                            if (dragged)
                            {
                                newContentPos -= lastPos - ImGui.GetMousePos();

                                lastPos = ImGui.GetMousePos();
                            }
                            else
                            {
                                dragged = true;
                                lastPos = ImGui.GetMousePos();
                            }
                        }
                        
                    }
                    else
                    {
                        dragged = false;
                        lastPos = ImGui.GetMousePos();
                    }

                    if (x != mapSize.X - 1)
                    {
                        ImGui.SameLine();
                    }
                    
                }
            }
            ImGui.PopStyleVar();
            ImGui.PopStyleVar();

            ImGui.SetCursorPos(ImGui.GetWindowPos());
            ImGui.End();
            contentPosition = newContentPos;
        }
    }
}