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

namespace AdvancedEdit.Types{
    /// <summary>
    /// Palette of BGR555 colors
    /// </summary>
    public struct Palette
    {
        public BgrColor[] palette;
        public Palette(BgrColor[] pal)
        {
            this.palette = pal;
        }
        public Palette(byte[] bpal)
        {
            BgrColor[] pal = new BgrColor[(bpal.Length) / 2];
            for (int i = 0; i < bpal.Length; i += 2)
            {
                ushort color = (ushort)((bpal[i + 1] << 8) | bpal[i]);
                pal[(int)(i / 2)] = new BgrColor(color);
            }
            this.palette = pal;
        }
        public BgrColor this[int i]
        {
            get { 
                if (i > palette.Length) i = i % palette.Length;
                return this.palette[i]; 
            }
            set { 
                this.palette[i] = value; 
            }
        }

    }
}