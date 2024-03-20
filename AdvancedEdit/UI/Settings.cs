using ImGuiNET;

namespace AdvancedEdit.UI;

class Settings{
    public static void Draw(){
        ImGui.Begin("Settings");

        if (ImGui.Button("Change Style")){
            ImGui.ShowStyleEditor();
        }

        ImGui.End();
    }
}