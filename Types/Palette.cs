using static SDL2.SDL;

namespace MkscEdit.Types{
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
            get => this.palette[i];
            set => this.palette[i] = value;
        }

    }
}