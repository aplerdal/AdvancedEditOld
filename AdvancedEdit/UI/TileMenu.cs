using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedEdit.Types;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AdvancedEdit.UI
{
    class TileImport
    {
        static readonly int maxPalSize = 64;
        public static Tile[] FromTexture(Texture2D tileTexure){
            List<BgrColor> palette = new List<BgrColor>();
            palette.Add(new BgrColor(255,0,255)); // add "transparent" magenta color at start to use for transparency
            Color[] tileColors = new Color[tileTexure.Width*tileTexure.Height];
            Console.WriteLine(tileColors.Length);
            tileTexure.GetData(tileColors);
            
            //Check to make sure layout is valid
            if (!(tileTexure.Width == 128 && tileTexure.Height == 128)){
                // TODO;
                // Prompt with error
                // same as max size with differenst message
                throw new Exception("Wrong image size! must be 256 total tiles in an 16x16 tile grid! (resolution of 128x128)");
            }
            
            // Load every color in image into palette
            for (int y = 0; y < 128; y++){
                for (int x = 0; x < 128; x++){
                    var bColor = tileColors[y*128+x].toBgrColor();
                    if (!palette.Contains(bColor)){
                        palette.Add(bColor);
                    };
                }
            }

            int palSize = palette.Count;
            if (palSize > maxPalSize) {
                // TODO:
                // If the length of the palette is too large (>maxPalSize)
                // create popup(?) that alerts them the image is invalid.
                throw new Exception("Wrong image colors! make sure you only have 64 or fewer total colors. (Using jpg files may cause this)");         
            } else if (palSize < maxPalSize) {
                while (palSize < maxPalSize) {
                    palSize++;
                    palette.Add(new BgrColor((ushort)0b0)); // fill empty palette entries with blank color
                }
            }

            //Then load image data and find closest palette entry for each color on each tile.
            Tile[] tilePal = new Tile[256];
            for (int ty = 0; ty < 16; ty++){      // Loops over tiles
                for (int tx = 0; tx < 16; tx++){  // Loops over tiles
                    byte[,] tileIndicies = new byte[8,8];
                    for (int y = 0; y < 8; y++){    // loops in tiles
                        for (int x = 0; x < 8; x++){// loops in tiles
                            int xpos = (tx * 8) + x;
                            int ypos = (ty * 8) + y;
                            tileIndicies[x,y] = (byte)palette.IndexOf(tileColors[ypos*128+xpos].toBgrColor());
                        }
                    }
                    tilePal[ty*16+tx] = new Tile(tileIndicies,new Palette(palette.ToArray()));
                }
            }
            return tilePal;
        }
        // public void Draw(){
        //     ImGui.BeginPopup("Import Tiles:");
            
        //     ImGui.Text("Padding and offset currently broken.");
        //     ImGui.InputFloat2("Tile Offset", ref offset);
        //     offset = new(MathF.Round(offset.X),MathF.Round(offset.Y));

        //     ImGui.InputFloat2("Tile Padding", ref padding);
        //     padding = new(MathF.Round(padding.X),MathF.Round(padding.Y));

        //     ImGui.EndPopup();
        // }
    }
}
