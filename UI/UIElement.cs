using AdvancedEdit.Types;
using static SDL2.SDL;

namespace AdvancedEdit.UI
{
    abstract class UIElement
    {
        public SDL_Rect ElementPosition;
        public abstract void Draw();
        public abstract void Update();
        public abstract void Events(SDL_Event e);

    }
}
