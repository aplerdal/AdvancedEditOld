using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using AdvancedEdit.Types;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;

namespace AdvancedEdit.UI
{
    class TileImport
    {
        static readonly int maxPalSize = 64;
        int width = 8;
        int height = 8;
        Vector2 padding = new(0,0);
        Vector2 offset = new(0,0);
        public TileImport(Texture2D tiles){
            List<BgrColor> palette = new List<BgrColor>();

            // Load every color in image into palette
            for (int y = 0; y < tiles.Height; y++){
                for (int x = 0; x < tiles.Width; x++){
                    var color = new BgrColor(/*Read pixel*/);
                    if (!palette.Contains(color)){
                        palette.Add(color);
                    };
                }
            }

            // If the length of the palette is too large (>maxPalSize) combine the most similar colors until there are only maxPalSize
            int palSize = palette.Count;
            Dictionary<int,Vector2I> differences = new Dictionary<int, Vector2I>();
            if (palSize > maxPalSize) {
                for (int i = 0; i < palSize; i++){
                    for (int j = i+1; j < palSize; j++){
                        differences.Add(
                            Math.Abs(palette[i].b-palette[j].b)+Math.Abs(palette[i].g-palette[j].g)+Math.Abs(palette[i].r-palette[j].r),
                            new(i,j)
                        );
                    }
                }
                foreach (var d in differences.Order()){ // Goal: take list sorted from smallest to largest and remove the smallest differences until it is the correct size.
                    differences.Remove(d.Value.Y);
                    palSize--;
                    if (palSize<=maxPalSize){
                        break;
                    }
                }
            } else if (palSize < maxPalSize) {
                while (palSize < maxPalSize) {
                    palSize++;
                    palette.Add(new BgrColor((ushort)0b0)); // fill empty palette entries with blank color
                }
            }

            //Then load image data and find closest palette entry for each color on each tile.
        }
        public void Draw(){
            ImGui.BeginPopup("Import Tiles:");
            
            ImGui.Text("Padding and offset currently broken.");
            ImGui.InputFloat2("Tile Offset", ref offset);
            offset = new(MathF.Round(offset.X),MathF.Round(offset.Y));

            ImGui.InputFloat2("Tile Padding", ref padding);
            padding = new(MathF.Round(padding.X),MathF.Round(padding.Y));

            ImGui.EndPopup();
        }
    }
}
