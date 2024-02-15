using AdvancedEdit.Types;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace AdvancedEdit.UI
{
    class ToolbarItem {
        public SDL_Rect elementPosition;
        public string displayName;
        public List<ToolbarItem>? Children;
        public ToolbarItem(string name, OnClick onClick){
            displayName = name;
        }
        public ToolbarItem(string name, List<ToolbarItem> children){
            displayName = name;
            Children = children;
        }
        public void Draw(){
            // Add custom font renderer
        }
    }
}
