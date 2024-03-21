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