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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdvancedEdit.Types
{
    /// <summary>
    /// GBA native BGR color format (Blue Green Red)
    /// where each color is 5 bits
    /// </summary>
    public struct BgrColor
    {
        public byte b;
        public byte g;
        public byte r;
        public BgrColor(ushort bgr555)
        {
            this.b = (byte)((bgr555 & 0b01111100_00000000) >> 10);
            this.g = (byte)((bgr555 & 0b00000011_11100000) >> 5);
            this.r = (byte)((bgr555 & 0b00000000_00011111) >> 0);
        }
        public BgrColor(byte b, byte g, byte r)
        {
            this.b = (b);
            this.g = (g);
            this.r = (r);
        }
        public Color ToColor()
        {
            return new Color((byte)(r * 8), (byte)(g * 8), (byte)(b * 8),(byte)255);
        }
    }
}