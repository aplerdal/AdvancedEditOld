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
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AdvancedEdit.Types {
    public class Tile
    {
        public Palette palette;
        public byte[,] indicies;
        public Tile(byte[,] indicies, Palette palette)
        {
            this.palette = palette;
            this.indicies = indicies;
        }
        public static Tile[] GenerateTiles(byte[] indicies, Palette palette)
        {
            int tileno = (int)Math.Ceiling((decimal)indicies.Length / (decimal)64);
            Tile[] tiles = new Tile[tileno];
            for (int t = 0; t < tileno; t++)
            {
                byte[,] tile = new byte[8, 8];
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if ((y * 8 + x) < indicies.Length)
                        {
                            tile[x, y] = indicies[y * 8 + (x + t * 64)];
                        }
                        else
                        {
                            tile[x, y] = 0;
                        }
                    }
                }
                tiles[t] = new Tile(tile, palette);
            }
            return tiles;
        }
        public static byte[] GetTileBytes(Tile[] tiles)
        {
            int totalIndices = tiles.Length * 64;
            List<byte> indicesList = new List<byte>();

            foreach (Tile tile in tiles)
            {
                byte[,] tileData = tile.indicies;

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (indicesList.Count < totalIndices)
                        {
                            indicesList.Add(tileData[x, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return indicesList.ToArray();
        }
        public Texture2D ToImage(GraphicsDevice device)
        {
            Texture2D texture = new Texture2D(device,8,8);

            Color[] data = new Color[64];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    data[x+y*8] = palette[indicies[x, y]].ToColor();
                }
            }
            texture.SetData(data);
            return texture;
        }
    }
}