using AdvancedEdit.TrackData;
using AdvancedEdit.Types;
namespace AdvancedEdit.UI;

class UiManager{
    TrackId currentTrack {get; set;}
    TilePalette tilePalette;
    public UiManager(){
        Init();
    }
    public void Init(){
        tilePalette = new TilePalette();
        tilePalette.SetTrack(TrackId.PeachCircuit);
    }
    public void Draw(){
        if (AdvancedEditor.loaded){
            tilePalette.Draw();
        }
    }
}