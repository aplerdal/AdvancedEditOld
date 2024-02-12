using MkscEdit.Types;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace MkscEdit.UI
{
    // class TextButton
    // {
    //     public SDL_Rect elementPosition;
    //     public IntPtr texture;
    //     public OnClick onClick;
    //     public Button button;

    //     public TextButton(SDL_Rect elementPosition, string text, SDL_Color textColor, SDL_Color bgColor, OnClick onClick)
    //     {
    //         this.elementPosition = elementPosition;
    //         this.onClick = onClick;
    //         IntPtr t = TTF_RenderUTF8_Shaded(Program.Sans, text, textColor, bgColor);
            
    //         texture = SDL_CreateTextureFromSurface(Program.Renderer, t);
            
    //         button = new Button(elementPosition,texture,onClick);
    //     }
    //     public void Draw()
    //     {
    //         button.Draw();
    //     }
    //     public void IsClicked(int x, int y)
    //     {
    //         if (elementPosition.Contains(x, y))
    //         {
    //             button.onClick();
    //         }
    //     }
    // }
}
