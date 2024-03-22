#region LICENSE
/*
Copyright(C) 2024 Andrew Lerdal

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
#endregion

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
