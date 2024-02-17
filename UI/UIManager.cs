using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedEdit.UI
{
    class UIManager
    {
        List<UIElement> elements = new List<UIElement>();
        public void AddElement(UIElement element)
        {
            elements.Add(element);
        }
        public void UpdateElements()
        {
            foreach (UIElement element in elements)
            {
                element.Update();
            }
        }
        public void DrawElements()
        {
            foreach (UIElement element in elements)
            {
                element.Draw();
            }
        }
        public void ElementEvents(SDL2.SDL.SDL_Event e)
        {
            foreach (UIElement element in elements)
            {
                element.Events(e);
            }
        }
    }
}
