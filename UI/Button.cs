using MkscEdit.Types;
using static SDL2.SDL;
namespace MkscEdit.UI{
    class Button {
        public delegate void OnClick();
        public IntPtr renderer;
        public SDL_Rect elementPosition;
        public IntPtr texture;
        public OnClick onClick;

        public Button(IntPtr renderer, SDL_Rect elementPosition, IntPtr texture, OnClick onClick){
            this.renderer = renderer;
            this.elementPosition= elementPosition;
            this.texture = texture;
            this.onClick = onClick;
        }
        public void Draw(){
            SDL_RenderCopy(renderer,texture,IntPtr.Zero,ref elementPosition);
        }
        public void IsClicked(int x, int y){
            if (elementPosition.Contains(x,y)){
                onClick();
            }
        }

    }
}