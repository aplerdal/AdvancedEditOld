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
                            }
                            AdvancedEditor.loaded = true;
                        }
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S"))
                    {
                        if (AdvancedEditor.loaded)
                        {
                            File.WriteAllBytes("MkscModified.gba", Track.CompileRom(AdvancedEditor.tracks));
                        }
                    }
                    if (ImGui.MenuItem("Save as.."))
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
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }
        }
    }
}
