using AdvancedEdit.Types;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static System.Net.Mime.MediaTypeNames;

namespace AdvancedEdit.UI
{
    class Dropdown<T> : UIElement
    {
        SDL_Rect OpenedPosition;
        List<IntPtr> textures;
        public bool Opened;
        public int Selected;
        public SDL_Color bgColor;

        List<T> items;
        public unsafe Dropdown(SDL_Rect elementPosition, SDL_Color bgColor, List<T> values) 
        {
            this.bgColor = bgColor;
            ElementPosition = elementPosition;
            items = values;
            textures = new List<IntPtr>();
            foreach (var item in items)
            {
                IntPtr surface = SDL_CreateRGBSurface(0,elementPosition.w,elementPosition.h,32,0,0,0,0);
                SDL_FillRect(surface, IntPtr.Zero, SDL_MapRGBA(((SDL_Surface*)surface.ToPointer())->format,bgColor.r,bgColor.g,bgColor.b,bgColor.a));
                IntPtr text = SDL_ttf.TTF_RenderUTF8_LCD(Program.Font, item.ToString() == null ? "None" : item.ToString(), new SDL_Color(255,255,255,255), bgColor);
                SDL_BlitSurface(text, IntPtr.Zero, surface, IntPtr.Zero);
                textures.Add(SDL_CreateTextureFromSurface(Program.Renderer,surface));
            }
            OpenedPosition = new SDL_Rect { x = elementPosition.x, y = elementPosition.y, w = elementPosition.w, h = elementPosition.h * items.Count };
        }
        public override void Draw()
        {
            SDL_RenderCopy(Program.Renderer, textures[Selected], IntPtr.Zero, ref ElementPosition);
            if (Opened)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var pos = new SDL_Rect { x = ElementPosition.x, y = ElementPosition.y + ElementPosition.h * i, w = ElementPosition.w, h = ElementPosition.h };
                    SDL_RenderCopy(Program.Renderer, textures[i], IntPtr.Zero, ref pos);
                }
            }
        }
        public override void Events(SDL_Event e)
        {
            switch (e.type) 
            {
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (Opened && OpenedPosition.Contains(e.motion.x, e.motion.y))
                    {
                        Selected = (int)((double)(e.motion.y - ElementPosition.y)/(double)ElementPosition.h);
                        Opened = !Opened;
                    }
                    else if (ElementPosition.Contains(e.motion.x, e.motion.y))
                    {
                        Opened = !Opened;
                    }
                    break;
            }
        }
        public override void Update()
        {
        }
    }
}
