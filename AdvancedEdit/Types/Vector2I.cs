using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvancedEdit.Types
{
    struct Vector2I
    {
        public int X; public int Y;
        public Vector2I(int x, int y)
        {
            X = x; Y = y;
        }
        public static Vector2I operator +(Vector2I a) => a;
        public static Vector2I operator +(Vector2I a, Vector2I b) => new Vector2I(a.X + b.X, a.Y + b.Y);
        public static Vector2I operator -(Vector2I a) => new Vector2I(-a.X,-a.Y);
        public static Vector2I operator -(Vector2I a, Vector2I b) => new Vector2I(a.X - b.X, a.Y - b.Y);
    }
}
