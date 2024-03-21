using MonoGame.ImGuiNet;
using ImGuiNET;
using NativeFileDialog.Extended;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.IO;
using AdvancedEdit.TrackData;

namespace AdvancedEdit.UI
{
    class MenuBar
    {
        public static void Draw()
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

                if (ImGui.BeginMenu("Track"))
                {
                    ImGui.BeginDisabled(!AdvancedEditor.loaded);
                    if (ImGui.BeginMenu("Set Track")){
                        if(ImGui.BeginMenu("Mushroom Cup")){
                            if (ImGui.MenuItem("Peach Circuit")){

                            }
                            if (ImGui.MenuItem("Shy Guy Beach")){

                            }
                            if (ImGui.MenuItem("Riverside Park")){

                            }
                            if (ImGui.MenuItem("Bowser Castle 1")){

                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Flower Cup")){
                            if (ImGui.MenuItem("Mario Circuit")){

                            }
                            if (ImGui.MenuItem("Boo Lake")){

                            }
                            if (ImGui.MenuItem("Cheese Land")){

                            }
                            if (ImGui.MenuItem("Bowser Castle")){

                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Lightning Cup")){
                            if (ImGui.MenuItem("Luigi Circuit")){

                            }
                            if (ImGui.MenuItem("Sky Garden")){

                            }
                            if (ImGui.MenuItem("Cheep Cheep Island")){

                            }
                            if (ImGui.MenuItem("Sunset Wilds")){

                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Star Cup")){
                            if (ImGui.MenuItem("Snow Land")){

                            }
                            if (ImGui.MenuItem("Ribbon Road")){

                            }
                            if (ImGui.MenuItem("Yoshi Desert")){

                            }
                            if (ImGui.MenuItem("Bowser Castle 3")){

                            }
                            ImGui.EndMenu();
                        }

                        if (ImGui.BeginMenu("Special Cup")){
                            if (ImGui.MenuItem("Lakeside Park")){

                            }
                            if (ImGui.MenuItem("Broken Pier")){

                            }
                            if (ImGui.MenuItem("Bowser Castle 4")){

                            }
                            if (ImGui.MenuItem("Rainbow Road")){

                            }
                            ImGui.EndMenu();
                        }
                        
                        ImGui.EndMenu();
                    }
                    ImGui.EndDisabled();
                    
                    ImGui.EndMenu();
                }
                

                ImGui.EndMainMenuBar();
            }
        }
    }
}
