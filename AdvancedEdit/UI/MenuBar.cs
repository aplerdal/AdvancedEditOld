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

using MonoGame.ImGuiNet;
using ImGuiNET;
using NativeFileDialog.Extended;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.IO;
using System.Drawing;
using AdvancedEdit.TrackData;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Graphics;
using AdvancedEdit.Types;

namespace AdvancedEdit.UI
{
    class MenuBar
    {
        public static void Draw(ref TrackId trackId, ref Tile[] tiles)
        {

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open", "Ctrl+O"))
                    {
                        string str = NFD.OpenDialog("", new Dictionary<string, string>() { { "Game Boy Advance ROM", "gba" } });
                        if (str != null)
                        {
                            if (Rom.OpenRom(str))
                            {
                                Track.GenerateTracks();
                                AdvancedEditor.loaded = true;
                            }
                        }
                    }

                    ImGui.BeginDisabled(!AdvancedEditor.loaded);
                    if (ImGui.MenuItem("Save", "Ctrl+S"))
                    {
                        File.WriteAllBytes("MkscModified.gba", Track.CompileRom(AdvancedEditor.tracks));
                    }

                    if (ImGui.MenuItem("Save as..", "Ctrl+Shift+S"))
                    {
                        if (AdvancedEditor.loaded)
                        {
                            string str = NFD.SaveDialog("", "", new Dictionary<string, string>() { { "Game Boy Advance ROM", "gba" } });
                            if (str != null)
                            {
                                File.WriteAllBytes(str, Track.CompileRom(AdvancedEditor.tracks));
                            }
                        }
                    }
                    ImGui.EndDisabled();

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Undo", "Ctrl+Z")){
                        
                    }
                    if (ImGui.MenuItem("Redo", "Ctrl+Y")){

                    }
                    if (ImGui.MenuItem("Clear Track")){
                        
                    }
                }

                if (ImGui.BeginMenu("Track"))
                {
                    ImGui.BeginDisabled(!AdvancedEditor.loaded);
                    if (ImGui.BeginMenu("Set Track")){
                        if(ImGui.BeginMenu("Mushroom Cup")){
                            if (ImGui.MenuItem("Peach Circuit")){
                                trackId = TrackId.PeachCircuit;
                            }
                            if (ImGui.MenuItem("Shy Guy Beach")){
                                trackId = TrackId.ShyGuyBeach;
                            }
                            if (ImGui.MenuItem("Riverside Park")){
                                trackId = TrackId.RiversidePark;
                            }
                            if (ImGui.MenuItem("Bowser Castle 1")){
                                trackId = TrackId.BowserCastle1;
                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Flower Cup")){
                            if (ImGui.MenuItem("Mario Circuit")){
                                trackId = TrackId.MarioCircuit;
                            }
                            if (ImGui.MenuItem("Boo Lake")){
                                trackId = TrackId.BooLake;
                            }
                            if (ImGui.MenuItem("Cheese Land")){
                                trackId = TrackId.CheeseLand;
                            }
                            if (ImGui.MenuItem("Bowser Castle 2")){
                                trackId = TrackId.BowserCastle2;
                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Lightning Cup")){
                            if (ImGui.MenuItem("Luigi Circuit")){
                                trackId = TrackId.LuigiCircuit;
                            }
                            if (ImGui.MenuItem("Sky Garden")){
                                trackId = TrackId.SkyGarden;
                            }
                            if (ImGui.MenuItem("Cheep Cheep Island")){
                                trackId = TrackId.CheepCheepIsland;
                            }
                            if (ImGui.MenuItem("Sunset Wilds")){
                                trackId = TrackId.SunsetWilds;
                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Star Cup")){
                            if (ImGui.MenuItem("Snow Land")){
                                trackId = TrackId.SnowLand;
                            }
                            if (ImGui.MenuItem("Ribbon Road")){
                                trackId = TrackId.RibbonRoad;
                            }
                            if (ImGui.MenuItem("Yoshi Desert")){
                                trackId = TrackId.YoshiDesert;
                            }
                            if (ImGui.MenuItem("Bowser Castle 3")){
                                trackId = TrackId.BowserCastle3;
                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Special Cup")){
                            if (ImGui.MenuItem("Lakeside Park")){
                                trackId = TrackId.LakesidePark;
                            }
                            if (ImGui.MenuItem("Broken Pier")){
                                trackId = TrackId.BrokenPier;
                            }
                            if (ImGui.MenuItem("Bowser Castle 4")){
                                trackId = TrackId.BowserCastle4;
                            }
                            if (ImGui.MenuItem("Rainbow Road")){
                                trackId = TrackId.RainbowRoad;
                            }
                            ImGui.EndMenu();
                        }
                        
                        ImGui.EndMenu();
                    }
                    ImGui.EndDisabled();
                    
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Tiles"))
                {
                    if (ImGui.MenuItem("Import..."))
                    {
                        var path = NFD.OpenDialog("", new Dictionary<string, string>() { { "png", "png" }, { "bmp", "bmp" } });
                        if (path != ""){
                            Texture2D texture = Texture2D.FromFile(AdvancedEditor.gd, path);
                            tiles = TileImport.FromTexture(texture);
                        }
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("View"))
                {

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }
        }
    }
}
