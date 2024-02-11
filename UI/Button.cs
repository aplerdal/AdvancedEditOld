using MkscEdit.Types;
using static SDL2.SDL;
namespace MkscEdit.UI{
    public delegate void OnClick();
    class Button {
        public SDL_Rect elementPosition;
        public IntPtr texture;
        public OnClick onClick;

        public Button(SDL_Rect elementPosition, IntPtr texture, OnClick onClick){
            this.elementPosition= elementPosition;
            this.texture = texture;
            this.onClick = onClick;
        }
        public void Draw(){
            SDL_RenderCopy(Program.Renderer,texture,IntPtr.Zero,ref elementPosition);
        }
        public void IsClicked(int x, int y){
            if (elementPosition.Contains(x,y)){
                onClick();
            }
        }

    }
}