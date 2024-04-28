using ImGuiNET;
using AdvancedEdit.Types;
using System.Numerics;

namespace AdvancedEdit.UI;
class ToolManager{

    int tileSize = 16;
    Vector2I mapSize;
    bool dragged = false;
    Vector2 lastPos;

    Tool activeTool;
    int selectedTile;
    int[,] selectedArea;
    int[,] previewIndicies;
    Vector2 contentPosition = new Vector2(0,0);
    public ToolManager(){

        selectedTile = 0;
        selectedArea = new int[0,0];
    }
    public void ClickEvent(int x, int y, ref byte[,] indicies){

    }
    public void MouseUpEvent(int x, int y, ref byte[,] indicies){
        
    }
    public void DrawToolPreview(){
        
    }
}
public enum Tool{
    Pencil,
    Line,
    Curve,
    Rectangle,
    FilledRectangle,
    Oval,
    Bucket,
}