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

using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace AdvancedEdit.UI;

class UiManager{
    TilePalette tilePalette;
    TrackPanel trackPanel;

    //Track Variables
    public Texture2D[] tileObjects = new Texture2D[256];
    public Tile[] tiles = new Tile[256];
    public IntPtr[] tileTextures = new IntPtr[256];
    public byte[,] indicies = new byte[0,0];

    TrackId trackId;
    TrackId newTrack;

    public byte selectedTile;

    public UiManager(){
        selectedTile = 0;
    }
    public void Init(){
        tilePalette = new TilePalette();
        trackPanel = new TrackPanel();
        SetTrack(TrackId.PeachCircuit);
    }
    public void Draw(){
        MenuBar.Draw(ref newTrack, ref tiles);
        if(newTrack != trackId)
        {
            SetTrack(newTrack);
        }
        if (AdvancedEditor.intialized){
            if(tiles != AdvancedEditor.tracks[(int)trackId].Tiles){
                SetTiles(tiles);
            }
            tilePalette.Draw(tileTextures, ref selectedTile);
            trackPanel.Draw(tileTextures, indicies, ref selectedTile);
        }
    }
    public void SetTiles(Tile[] tiles)
    {
        this.tiles = tiles;
        
        AdvancedEditor.tracks[(int)trackId].Tiles = tiles;
        for (int i = 0; i < AdvancedEditor.tracks[(int)trackId].Tiles.Length; i++)
        {
            //Load Tile texture
            if (tileTextures[i] != IntPtr.Zero)
            {
                AdvancedEditor.GuiRenderer.UnbindTexture(tileTextures[i]);
            }
            Texture2D tile = tiles[i].ToImage(AdvancedEditor.gd);
            tileTextures[i] = AdvancedEditor.GuiRenderer.BindTexture(tile);

            tiles[i] = tiles[i];
        }
    }

    /// <summary>
    /// Sets the tiles, palette, and indicies to the given track's
    /// </summary>
    /// <param name="trackId">Id of new track</param>
    public void SetTrack(TrackId trackId)
    {
        this.newTrack = trackId;
        this.trackId = trackId;
        this.tiles = AdvancedEditor.tracks[(int)trackId].Tiles;

        indicies = AdvancedEditor.tracks[(int)trackId].Indicies;
        for (int i = 0; i < AdvancedEditor.tracks[(int)trackId].Tiles.Length; i++)
        {
            //Load Tile texture
            if (tileTextures[i] != IntPtr.Zero)
            {
                AdvancedEditor.GuiRenderer.UnbindTexture(tileTextures[i]);
            }
            Texture2D tile = AdvancedEditor.tracks[(int)trackId].Tiles[i].ToImage(AdvancedEditor.gd);
            tileTextures[i] = AdvancedEditor.GuiRenderer.BindTexture(tile);

            tileObjects[i] = tile;
        }
    }
}