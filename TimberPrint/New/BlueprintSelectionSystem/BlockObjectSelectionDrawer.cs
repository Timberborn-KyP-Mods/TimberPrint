using System.Collections.Generic;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.SelectionSystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlockObjectSelectionDrawer(
    RectangleBoundsDrawer rectangleBoundsDrawer,
    RectangleBoundsDrawer topRectangleBoundsDrawer,
    RollingHighlighter rollingHighlighter,
    Color blockObjectHighlightColor)
{
    private Vector3Int _start;
    
    private Vector3Int _end;

    private int _height;
    
    private bool _selectingArea;

    public void Draw(
        IEnumerable<BlockObject> blockObjects,
        Vector3Int start,
        Vector3Int end,
        int height,
        bool selectingArea)
    {
        _start = start;
        _end = end;
        _selectingArea = selectingArea;
        _height = height;
        Draw();
        rollingHighlighter.HighlightPrimary(blockObjects, blockObjectHighlightColor);
    }

    public void StopDrawing() => rollingHighlighter.UnhighlightAllPrimary();

    private void Draw()
    {
        if (!_selectingArea)
        {
            return;
        }
            
        rectangleBoundsDrawer.DrawOnLevel(_start.XY(), _end.XY(), _start.z);

        if (_height > 0)
        {
            topRectangleBoundsDrawer.DrawOnLevel(_start.XY(), _end.XY(), _start.z + _height);
        }
    }
}