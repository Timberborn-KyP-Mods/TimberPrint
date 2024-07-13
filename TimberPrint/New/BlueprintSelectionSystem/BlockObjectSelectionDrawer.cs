using System.Collections.Generic;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.SelectionSystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlockObjectSelectionDrawer
{
    private readonly RectangleBoundsDrawer _rectangleBoundsDrawer;
    
    private readonly RollingHighlighter _rollingHighlighter;
    
    private readonly Color _blockObjectHighlightColor;
    
    private Vector3Int _start;
    
    private Vector3Int _end;

    private int _height;
    
    private bool _selectingArea;

    public BlockObjectSelectionDrawer(
        RectangleBoundsDrawer rectangleBoundsDrawer,
        RollingHighlighter rollingHighlighter,
        Color blockObjectHighlightColor)
    {
        _rectangleBoundsDrawer = rectangleBoundsDrawer;
        _rollingHighlighter = rollingHighlighter;
        _blockObjectHighlightColor = blockObjectHighlightColor;
    }

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
        _rollingHighlighter.HighlightPrimary(blockObjects, _blockObjectHighlightColor);
    }

    public void StopDrawing() => _rollingHighlighter.UnhighlightAllPrimary();

    private void Draw()
    {
        if (!_selectingArea)
            return;
        _rectangleBoundsDrawer.DrawOnLevel(_start.XY(), _end.XY(), _start.z);
        
        _rectangleBoundsDrawer.DrawOnLevel(_start.XY(), _end.XY(), _start.z + _height);
    }
}