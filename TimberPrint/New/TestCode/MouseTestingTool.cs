using System.Collections.Generic;
using Timberborn.BlockSystem;
using Timberborn.CursorToolSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using TimberPrint.New.BlueprintSelectionSystem;
using UnityEngine;

namespace TimberPrint.New.TestCode;

public class MouseTestingTool : Tool, IInputProcessor, ILoadableSingleton
{
    private readonly InputService _inputService;

    private readonly CursorCoordinatesPicker _cursorCoordinatesPicker;
    
    private readonly BlockObjectSelectionDrawerFactory _blockObjectSelectionDrawerFactory;
    
    private BlockObjectSelectionDrawer _blockObjectSelectionDrawer = null!;
    
    private Vector3Int? _pointA;
    
    private Vector3Int? _pointB;

    private int _height;

    public MouseTestingTool(InputService inputService, CursorCoordinatesPicker cursorCoordinatesPicker, BlockObjectSelectionDrawerFactory blockObjectSelectionDrawerFactory)
    {
        _inputService = inputService;
        _cursorCoordinatesPicker = cursorCoordinatesPicker;
        _blockObjectSelectionDrawerFactory = blockObjectSelectionDrawerFactory;
    }
    
    public bool ProcessInput()
    {
        if(_inputService.IsKeyDown("PointA"))
        {
            _pointA = GetCursorCoordinates()?.TileCoordinates;
        }
        
        if(_inputService.IsKeyDown("PointB"))
        {
            _pointB = GetCursorCoordinates()?.TileCoordinates;
        }
        
        if(_inputService.IsKeyDown("IncreaseHeight"))
        {
            _height += 1;
        }
        
        if(_inputService.IsKeyDown("DecreaseHeight"))
        {
            _height -= 1;
        }
        
        if (_pointA.HasValue && _pointB.HasValue)
        {
            _blockObjectSelectionDrawer.Draw(new List<BlockObject>(), _pointA.Value, _pointB.Value, _height, true);
        }
        
        Debug.LogWarning($"PointA: {_pointA}, PointB: {_pointB}, Height: {_height}");
        
        return false;
    }
    
    private CursorCoordinates? GetCursorCoordinates()
    {
        return _cursorCoordinatesPicker.CursorCoordinates();
    }

    public override void Enter()
    {
        _inputService.AddInputProcessor(this);
    }

    public override void Exit()
    {
        _inputService.RemoveInputProcessor(this);
    }

    public void Load()
    {
        _blockObjectSelectionDrawer = _blockObjectSelectionDrawerFactory.Create(Color.blue, Color.green, Color.red);
    }
}