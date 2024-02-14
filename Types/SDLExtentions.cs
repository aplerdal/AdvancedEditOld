using static SDL2.SDL;
namespace AdvancedEdit.Types{
    public static class ExtensionMethods{
        public static bool Contains(this SDL_Rect rect, int x, int y){
            if ( x > rect.x && y > rect.y && x < rect.x+rect.w && y < rect.y+rect.h) return true;
            return false;
        }
    }
}